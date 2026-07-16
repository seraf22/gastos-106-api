namespace Casa106.Application.DTOs;

public class CategoriaDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string TipoMovimiento { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public bool Activa { get; set; }
    public int Orden { get; set; }
}
