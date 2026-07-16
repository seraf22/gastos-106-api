using Microsoft.AspNetCore.Mvc;
using Casa106.Application.Abstractions;
using Casa106.Domain.Entities;
using Casa106.Domain.Enumerations;

namespace Casa106.Api.Controllers;

[ApiController]
[Route("api/analisis")]
public class AnalisisController : ControllerBase
{
    private readonly IFinancialDocumentAnalyzer _analyzer;
    private readonly IDocumentStorage _storage;
    private readonly IMovimientoRepository _movimientoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IPropiedadRepository _propiedadRepository;

    public AnalisisController(
        IFinancialDocumentAnalyzer analyzer,
        IDocumentStorage storage,
        IMovimientoRepository movimientoRepository,
        ICategoriaRepository categoriaRepository,
        IPropiedadRepository propiedadRepository)
    {
        _analyzer = analyzer;
        _storage = storage;
        _movimientoRepository = movimientoRepository;
        _categoriaRepository = categoriaRepository;
        _propiedadRepository = propiedadRepository;
    }

    [HttpPost("texto")]
    public async Task<ActionResult<AnalyzeResponse>> AnalyzarTexto(
        [FromBody] AnalyzeTextRequest request,
        [FromQuery] bool guardar = false,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Texto))
            return BadRequest("El texto no puede estar vacío");

        var resultado = await _analyzer.AnalyzeTextAsync(request.Texto, cancellationToken);

        if (!guardar)
            return Ok(new AnalyzeResponse { Analisis = resultado, Guardado = false });

        var (ok, mensajeError, movimientoId) = await GuardarResultadoAsync(
            resultado,
            OrigenMovimiento.TextoIA,
            cancellationToken);

        if (!ok)
        {
            return BadRequest(new AnalyzeResponse
            {
                Analisis = resultado,
                Guardado = false,
                Mensaje = mensajeError
            });
        }

        return Ok(new AnalyzeResponse
        {
            Analisis = resultado,
            Guardado = true,
            MovimientoId = movimientoId,
            Mensaje = "Movimiento guardado correctamente"
        });
    }

    [HttpPost("documento")]
    public async Task<ActionResult<AnalyzeResponse>> AnalizarDocumento(
        IFormFile archivo,
        [FromQuery] bool guardar = false,
        CancellationToken cancellationToken = default)
    {
        if (archivo == null || archivo.Length == 0)
            return BadRequest("El archivo no puede estar vacío");

        // Validar extensión
        var extensión = Path.GetExtension(archivo.FileName).ToLowerInvariant();
        var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
        if (!extensionesPermitidas.Contains(extensión))
            return BadRequest("Formato de archivo no permitido");

        // Validar tamaño (máx 10 MB)
        const long maxTamaño = 10 * 1024 * 1024;
        if (archivo.Length > maxTamaño)
            return BadRequest("El archivo es demasiado grande");

        using var ms = new MemoryStream();
        await archivo.CopyToAsync(ms, cancellationToken);
        var contenido = ms.ToArray();

        var resultado = await _analyzer.AnalyzeDocumentAsync(
            contenido,
            archivo.FileName,
            archivo.ContentType ?? "application/octet-stream",
            cancellationToken);

        if (!guardar)
            return Ok(new AnalyzeResponse { Analisis = resultado, Guardado = false });

        var (ok, mensajeError, movimientoId) = await GuardarResultadoAsync(
            resultado,
            OrigenMovimiento.ImagenIA,
            cancellationToken);

        if (!ok)
        {
            return BadRequest(new AnalyzeResponse
            {
                Analisis = resultado,
                Guardado = false,
                Mensaje = mensajeError
            });
        }

        return Ok(new AnalyzeResponse
        {
            Analisis = resultado,
            Guardado = true,
            MovimientoId = movimientoId,
            Mensaje = "Movimiento guardado correctamente"
        });
    }

    private async Task<(bool Ok, string? MensajeError, Guid? MovimientoId)> GuardarResultadoAsync(
        DetectedTransactionDto resultado,
        OrigenMovimiento origen,
        CancellationToken cancellationToken)
    {
        var propiedad = await _propiedadRepository.GetActivaAsync(cancellationToken);
        if (propiedad == null)
            return (false, "No hay propiedad activa", null);

        if (!resultado.Monto.HasValue || resultado.Monto.Value <= 0)
            return (false, "No se pudo guardar: monto inválido detectado por el análisis", null);

        if (string.IsNullOrWhiteSpace(resultado.Tipo) ||
            !Enum.TryParse<TipoMovimiento>(resultado.Tipo, true, out var tipoMovimiento))
            return (false, "No se pudo guardar: tipo de movimiento no detectado", null);

        var categorias = (await _categoriaRepository.GetAllActivasAsync(cancellationToken)).ToList();

        Categoria? categoria = null;
        if (!string.IsNullOrWhiteSpace(resultado.Categoria))
        {
            categoria = categorias.FirstOrDefault(c =>
                c.TipoMovimiento == tipoMovimiento &&
                string.Equals(c.Nombre, resultado.Categoria, StringComparison.OrdinalIgnoreCase));
        }

        categoria ??= categorias
            .Where(c => c.TipoMovimiento == tipoMovimiento)
            .OrderBy(c => c.Orden)
            .FirstOrDefault();

        if (categoria == null)
            return (false, "No se pudo guardar: no existe una categoría activa compatible", null);

        var movimiento = new Movimiento
        {
            Id = Guid.NewGuid(),
            PropiedadId = propiedad.Id,
            CategoriaId = categoria.Id,
            Tipo = tipoMovimiento,
            Estado = EstadoMovimiento.PendienteRevision,
            Origen = origen,
            FechaMovimiento = resultado.FechaMovimiento?.ToDateTime(TimeOnly.MinValue) ?? DateTime.UtcNow,
            PeriodoDesde = resultado.PeriodoDesde,
            PeriodoHasta = resultado.PeriodoHasta,
            Monto = resultado.Monto.Value,
            Descripcion = string.IsNullOrWhiteSpace(resultado.Descripcion)
                ? resultado.Categoria ?? "Movimiento detectado por análisis"
                : resultado.Descripcion,
            Proveedor = resultado.Proveedor,
            MetodoPago = resultado.MetodoPago,
            FechaCreacion = DateTime.UtcNow
        };

        await _movimientoRepository.AddAsync(movimiento, cancellationToken);
        return (true, null, movimiento.Id);
    }
}

public class AnalyzeTextRequest
{
    public string Texto { get; set; } = string.Empty;
}

public class AnalyzeResponse
{
    public DetectedTransactionDto Analisis { get; set; } = new();
    public bool Guardado { get; set; }
    public Guid? MovimientoId { get; set; }
    public string? Mensaje { get; set; }
}
