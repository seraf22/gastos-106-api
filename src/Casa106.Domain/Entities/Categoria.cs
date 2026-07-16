namespace Casa106.Domain.Entities;

using Enumerations;

public class Categoria
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public TipoMovimiento TipoMovimiento { get; set; }
    public GrupoCategoria Grupo { get; set; }
    public bool Activa { get; set; } = true;
    public int Orden { get; set; }

    // Relaciones
    public ICollection<Movimiento> Movimientos { get; set; } = [];
}
