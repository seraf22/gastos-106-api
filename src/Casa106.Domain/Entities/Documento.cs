namespace Casa106.Domain.Entities;

public class Documento
{
    public Guid Id { get; set; }
    public Guid PropiedadId { get; set; }
    public string NombreOriginal { get; set; } = string.Empty;
    public string NombreAlmacenado { get; set; } = string.Empty;
    public string RutaAlmacenamiento { get; set; } = string.Empty;
    public string TipoMime { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long TamanoBytes { get; set; }
    public string HashArchivo { get; set; } = string.Empty;
    public string? TextoExtraido { get; set; }
    public string? RespuestaIaJson { get; set; }
    public DateTime FechaCarga { get; set; }

    // Relaciones
    public Propiedad Propiedad { get; set; } = null!;
    public ICollection<Movimiento> Movimientos { get; set; } = [];
    public ICollection<ProcesamientoIA> Procesamientos { get; set; } = [];
}
