namespace Casa106.Application.Abstractions;

using Casa106.Domain.Entities;

public interface IMovimientoRepository
{
    Task<Movimiento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Movimiento>> GetByPropiedadAsync(Guid propiedadId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Movimiento> Items, int Total)> GetPagedAsync(
        Guid propiedadId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task AddAsync(Movimiento movimiento, CancellationToken cancellationToken = default);
    Task UpdateAsync(Movimiento movimiento, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
