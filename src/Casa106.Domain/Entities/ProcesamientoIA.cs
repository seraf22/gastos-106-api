namespace Casa106.Domain.Entities;

using Enumerations;

public class ProcesamientoIA
{
    public Guid Id { get; set; }
    public Guid DocumentoId { get; set; }
    public string Proveedor { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Solicitud { get; set; } = string.Empty;
    public string Respuesta { get; set; } = string.Empty;
    public decimal Confianza { get; set; }
    public EstadoProcesamientoIA Estado { get; set; }
    public string? Error { get; set; }
    public DateTime FechaProcesamiento { get; set; }

    // Relaciones
    public Documento Documento { get; set; } = null!;
}
