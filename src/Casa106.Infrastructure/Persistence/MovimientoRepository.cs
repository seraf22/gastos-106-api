namespace Casa106.Infrastructure.Persistence;

using Casa106.Application.Abstractions;
using Casa106.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class MovimientoRepository : IMovimientoRepository
{
    private readonly Casa106DbContext _context;

    public MovimientoRepository(Casa106DbContext context)
    {
        _context = context;
    }

    public async Task<Movimiento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Movimientos
            .Include(m => m.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Movimiento>> GetByPropiedadAsync(Guid propiedadId, CancellationToken cancellationToken = default)
    {
        return await _context.Movimientos
            .Where(m => m.PropiedadId == propiedadId)
            .Include(m => m.Categoria)
            .OrderByDescending(m => m.FechaMovimiento)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Movimiento> Items, int Total)> GetPagedAsync(
        Guid propiedadId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Movimientos
            .Where(m => m.PropiedadId == propiedadId)
            .Include(m => m.Categoria)
            .OrderByDescending(m => m.FechaMovimiento);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task AddAsync(Movimiento movimiento, CancellationToken cancellationToken = default)
    {
        await _context.Movimientos.AddAsync(movimiento, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Movimiento movimiento, CancellationToken cancellationToken = default)
    {
        _context.Movimientos.Update(movimiento);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var movimiento = await GetByIdAsync(id, cancellationToken);
        if (movimiento != null)
        {
            _context.Movimientos.Remove(movimiento);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
