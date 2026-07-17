namespace Casa106.Application.Abstractions;

using Casa106.Domain.Entities;

public interface IPropiedadRepository
{
    Task<Propiedad?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Propiedad?> GetActivaAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Propiedad propiedad, CancellationToken cancellationToken = default);
}
