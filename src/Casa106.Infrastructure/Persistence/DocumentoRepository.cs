namespace Casa106.Infrastructure.Persistence;

using Casa106.Application.Abstractions;
using Casa106.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class DocumentoRepository : IDocumentoRepository
{
    private readonly Casa106DbContext _context;

    public DocumentoRepository(Casa106DbContext context)
    {
        _context = context;
    }

    public async Task<Documento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Documentos.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<Documento?> GetByHashAsync(string hash, CancellationToken cancellationToken = default)
    {
        return await _context.Documentos
            .FirstOrDefaultAsync(d => d.HashArchivo == hash, cancellationToken);
    }

    public async Task AddAsync(Documento documento, CancellationToken cancellationToken = default)
    {
        await _context.Documentos.AddAsync(documento, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var documento = await GetByIdAsync(id, cancellationToken);
        if (documento != null)
        {
            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
