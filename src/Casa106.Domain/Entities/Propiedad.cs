namespace Casa106.Domain.Entities;

public class Propiedad
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? Unidad { get; set; }
    public bool Activa { get; set; } = true;
    public DateTime FechaCreacion { get; set; }

    // Relaciones
    public ICollection<Movimiento> Movimientos { get; set; } = [];
    public ICollection<Documento> Documentos { get; set; } = [];
    public ICollection<OcupacionMensual> Ocupaciones { get; set; } = [];
}
