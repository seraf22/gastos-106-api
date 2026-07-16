namespace Casa106.Domain.Entities;

public class OcupacionMensual
{
    public Guid Id { get; set; }
    public Guid PropiedadId { get; set; }
    public int Anio { get; set; }
    public int Mes { get; set; }
    public int NochesDisponibles { get; set; }
    public int NochesOcupadas { get; set; }
    public decimal IngresosAlojamiento { get; set; }
    public decimal? TarifaPromedio { get; set; }
    public string? Observaciones { get; set; }

    // Relaciones
    public Propiedad Propiedad { get; set; } = null!;
}
