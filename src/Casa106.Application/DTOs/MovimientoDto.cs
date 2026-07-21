namespace Casa106.Application.DTOs;

public class MovimientoDto
{
    public Guid Id { get; set; }
    public Guid PropiedadId { get; set; }
    public Guid CategoriaId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
    public DateTime FechaMovimiento { get; set; }
    // MesDevengo indica el mes al que corresponde el gasto (obligatorio)
    public DateOnly MesDevengo { get; set; }
    public DateOnly? PeriodoDesde { get; set; }
    public DateOnly? PeriodoHasta { get; set; }
    public decimal Monto { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string? Proveedor { get; set; }
    public string? MetodoPago { get; set; }
    public Guid? DocumentoId { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public string? CategoriaNombre { get; set; }
}
