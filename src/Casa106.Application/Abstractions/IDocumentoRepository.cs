namespace Casa106.Application.Abstractions;

using Casa106.Domain.Entities;

public interface IDocumentoRepository
{
    Task<Documento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Documento?> GetByHashAsync(string hash, CancellationToken cancellationToken = default);
    Task AddAsync(Documento documento, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
