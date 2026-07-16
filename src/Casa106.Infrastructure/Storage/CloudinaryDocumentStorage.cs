namespace Casa106.Infrastructure.Storage;

using Casa106.Application.Abstractions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

public class CloudinaryDocumentStorage : IDocumentStorage
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryDocumentStorage(IConfiguration configuration)
    {
        var cloudName = configuration["Cloudinary:CloudName"];
        var apiKey = configuration["Cloudinary:ApiKey"];
        var apiSecret = configuration["Cloudinary:ApiSecret"];

        if (string.IsNullOrWhiteSpace(cloudName) ||
            string.IsNullOrWhiteSpace(apiKey) ||
            string.IsNullOrWhiteSpace(apiSecret))
        {
            throw new InvalidOperationException("Cloudinary configuration is missing.");
        }

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<string> SaveAsync(byte[] fileContent, string fileName, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(fileContent);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = "casa106"
        };

        var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        if (result.Error is not null)
            throw new InvalidOperationException($"Cloudinary upload failed: {result.Error.Message}");

        return result.PublicId;
    }

    public async Task<byte[]?> ReadAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(storagePath))
            return null;

        var url = _cloudinary.Api.UrlImgUp.Secure(true).BuildUrl(storagePath);

        using var httpClient = new HttpClient();
        return await httpClient.GetByteArrayAsync(url, cancellationToken);
    }

    public async Task DeleteAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(storagePath))
            return;

        var deleteParams = new DeletionParams(storagePath)
        {
            ResourceType = ResourceType.Image
        };

        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Error is not null)
            throw new InvalidOperationException($"Cloudinary delete failed: {result.Error.Message}");
    }

    public Task<string> GetHashAsync(byte[] fileContent)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(fileContent);
        return Task.FromResult(Convert.ToHexString(hashedBytes).ToLowerInvariant());
    }
}
