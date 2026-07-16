namespace Casa106.Infrastructure.Storage;

using Casa106.Application.Abstractions;
using System.Security.Cryptography;

/// <summary>
/// Almacenamiento local de documentos.
/// Preparado para ser reemplazado por Azure Blob Storage.
/// </summary>
public class LocalDocumentStorage : IDocumentStorage
{
    private readonly string _basePath;

    public LocalDocumentStorage(string basePath = "uploads")
    {
        _basePath = basePath;
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveAsync(byte[] fileContent, string fileName, CancellationToken cancellationToken = default)
    {
        // Generar nombre único basado en GUID para evitar colisiones
        var uniqueName = $"{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
        var filePath = Path.Combine(_basePath, uniqueName);

        await File.WriteAllBytesAsync(filePath, fileContent, cancellationToken);
        return uniqueName; // Retornar solo el nombre, no la ruta completa
    }

    public async Task<byte[]?> ReadAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, storagePath);

        if (!File.Exists(fullPath))
            return null;

        return await File.ReadAllBytesAsync(fullPath, cancellationToken);
    }

    public async Task DeleteAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, storagePath);
        if (File.Exists(fullPath))
        {
            await Task.Run(() => File.Delete(fullPath), cancellationToken);
        }
    }

    public async Task<string> GetHashAsync(byte[] fileContent)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(fileContent);
            return Convert.ToHexString(hashedBytes).ToLower();
        }
    }
}
