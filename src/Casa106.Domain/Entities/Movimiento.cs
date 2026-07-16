namespace Casa106.Domain.Entities;

using Enumerations;

public class Movimiento
{
    public Guid Id { get; set; }
    public Guid PropiedadId { get; set; }
    public Guid CategoriaId { get; set; }
    public TipoMovimiento Tipo { get; set; }
    public EstadoMovimiento Estado { get; set; } = EstadoMovimiento.PendienteRevision;
    public OrigenMovimiento Origen { get; set; }
    public DateTime FechaMovimiento { get; set; }
    public DateOnly? PeriodoDesde { get; set; }
    public DateOnly? PeriodoHasta { get; set; }
    public decimal Monto { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string? Proveedor { get; set; }
    public string? MetodoPago { get; set; }
    public Guid? DocumentoId { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }

    // Relaciones
    public Propiedad Propiedad { get; set; } = null!;
    public Categoria Categoria { get; set; } = null!;
    public Documento? Documento { get; set; }
}
