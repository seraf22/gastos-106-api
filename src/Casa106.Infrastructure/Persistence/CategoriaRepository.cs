namespace Casa106.Infrastructure.Persistence;

using Casa106.Application.Abstractions;
using Casa106.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly Casa106DbContext _context;

    public CategoriaRepository(Casa106DbContext context)
    {
        _context = context;
    }

    public async Task<Categoria?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Categoria>> GetAllActivasAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categorias
            .Where(c => c.Activa)
            .OrderBy(c => c.TipoMovimiento)
            .ThenBy(c => c.Orden)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Categoria categoria, CancellationToken cancellationToken = default)
    {
        await _context.Categorias.AddAsync(categoria, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Categoria categoria, CancellationToken cancellationToken = default)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
