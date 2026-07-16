namespace Casa106.Application.DTOs;

public class CreateMovimientoRequest
{
    public string Tipo { get; set; } = string.Empty;
    public string CategoriaId { get; set; } = string.Empty;
    public DateTime FechaMovimiento { get; set; }
    public DateOnly? PeriodoDesde { get; set; }
    public DateOnly? PeriodoHasta { get; set; }
    public decimal Monto { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string? Proveedor { get; set; }
    public string? MetodoPago { get; set; }
}
