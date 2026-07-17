namespace Casa106.Application.Abstractions;

public interface IFinancialDocumentAnalyzer
{
    Task<DetectedTransactionDto> AnalyzeTextAsync(
        string text,
        CancellationToken cancellationToken = default);

    Task<DetectedTransactionDto> AnalyzeDocumentAsync(
        byte[] fileContent,
        string fileName,
        string mimeType,
        CancellationToken cancellationToken = default);
}

public class DetectedTransactionDto
{
    public string? Tipo { get; set; }
    public string? Categoria { get; set; }
    public decimal? Monto { get; set; }
    public DateOnly? FechaMovimiento { get; set; }
    public DateOnly? PeriodoDesde { get; set; }
    public DateOnly? PeriodoHasta { get; set; }
    public string? Proveedor { get; set; }
    public string? Descripcion { get; set; }
    public string? MetodoPago { get; set; }
    public decimal Confianza { get; set; }
    public List<string> Advertencias { get; set; } = [];
}
