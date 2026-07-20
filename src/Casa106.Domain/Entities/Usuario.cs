namespace Casa106.Domain.Entities;

using Enumerations;

public class Usuario
{
    public Guid Id { get; set; }
    public string User { get; set; } = null!;
    public string? Email { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string Rol { get; set; } = "User";
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? LastLogin { get; set; }
}
