using Microsoft.AspNetCore.Mvc;
using Casa106.Application.Abstractions;
using Casa106.Application.DTOs;
using Casa106.Domain.Entities;

namespace Casa106.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriasController(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias(CancellationToken cancellationToken)
    {
        var categorias = await _categoriaRepository.GetAllActivasAsync(cancellationToken);
        var dtos = categorias.Select(c => MapToDto(c)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaDto>> GetCategoria(Guid id, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id, cancellationToken);
        if (categoria == null)
            return NotFound();

        return Ok(MapToDto(categoria));
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> CreateCategoria(
        CreateCategoriaRequest request,
        CancellationToken cancellationToken)
    {
        var categoria = new Categoria
        {
            Id = Guid.NewGuid(),
            Nombre = request.Nombre,
            TipoMovimiento = Enum.Parse<Casa106.Domain.Enumerations.TipoMovimiento>(request.TipoMovimiento),
            Grupo = Enum.Parse<Casa106.Domain.Enumerations.GrupoCategoria>(request.Grupo),
            Activa = true,
            Orden = request.Orden
        };

        await _categoriaRepository.AddAsync(categoria, cancellationToken);

        return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, MapToDto(categoria));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategoria(
        Guid id,
        CreateCategoriaRequest request,
        CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id, cancellationToken);
        if (categoria == null)
            return NotFound();

        categoria.Nombre = request.Nombre;
        categoria.TipoMovimiento = Enum.Parse<Casa106.Domain.Enumerations.TipoMovimiento>(request.TipoMovimiento);
        categoria.Grupo = Enum.Parse<Casa106.Domain.Enumerations.GrupoCategoria>(request.Grupo);
        categoria.Orden = request.Orden;

        await _categoriaRepository.UpdateAsync(categoria, cancellationToken);

        return NoContent();
    }

    private static CategoriaDto MapToDto(Categoria c) => new()
    {
        Id = c.Id,
        Nombre = c.Nombre,
        TipoMovimiento = c.TipoMovimiento.ToString(),
        Grupo = c.Grupo.ToString(),
        Activa = c.Activa,
        Orden = c.Orden
    };
}

public class CreateCategoriaRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string TipoMovimiento { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public int Orden { get; set; }
}
