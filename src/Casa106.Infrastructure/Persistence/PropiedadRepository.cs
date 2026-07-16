namespace Casa106.Infrastructure.Persistence;

using Casa106.Application.Abstractions;
using Casa106.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class PropiedadRepository : IPropiedadRepository
{
    private readonly Casa106DbContext _context;

    public PropiedadRepository(Casa106DbContext context)
    {
        _context = context;
    }

    public async Task<Propiedad?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Propiedades.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Propiedad?> GetActivaAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Propiedades
            .FirstOrDefaultAsync(p => p.Activa, cancellationToken);
    }

    public async Task AddAsync(Propiedad propiedad, CancellationToken cancellationToken = default)
    {
        await _context.Propiedades.AddAsync(propiedad, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
