namespace Casa106.Application.Abstractions;

public interface IDocumentStorage
{
    Task<string> SaveAsync(byte[] fileContent, string fileName, CancellationToken cancellationToken = default);
    Task<byte[]?> ReadAsync(string storagePath, CancellationToken cancellationToken = default);
    Task DeleteAsync(string storagePath, CancellationToken cancellationToken = default);
    Task<string> GetHashAsync(byte[] fileContent);
}
