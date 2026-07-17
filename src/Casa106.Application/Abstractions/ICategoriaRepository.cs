namespace Casa106.Application.Abstractions;

using Casa106.Domain.Entities;

public interface ICategoriaRepository
{
    Task<Categoria?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Categoria>> GetAllActivasAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Categoria categoria, CancellationToken cancellationToken = default);
    Task UpdateAsync(Categoria categoria, CancellationToken cancellationToken = default);
}
