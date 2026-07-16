namespace Casa106.Infrastructure.FinancialAI;

using Casa106.Application.Abstractions;
using System;
using System.Globalization;

/// <summary>
/// Implementación fake/simulada del analizador de documentos financieros.
/// Útil para desarrollo y testing sin depender de un servicio externo.
/// Puede ser reemplazada posteriormente por OpenAI, Azure OpenAI, etc.
/// </summary>
public class FakeFinancialDocumentAnalyzer : IFinancialDocumentAnalyzer
{
    private static readonly Random Random = new();

    public Task<DetectedTransactionDto> AnalyzeTextAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        var result = ParseFinancialText(text);
        return Task.FromResult(result);
    }

    public Task<DetectedTransactionDto> AnalyzeDocumentAsync(
        byte[] fileContent,
        string fileName,
        string mimeType,
        CancellationToken cancellationToken = default)
    {
        // Para archivos, simulamos que extraemos texto y lo analizamos
        var result = ParseFinancialText($"Documento: {fileName}");
        return Task.FromResult(result);
    }

    private static DetectedTransactionDto ParseFinancialText(string text)
    {
        var lower = text.ToLower();
        var result = new DetectedTransactionDto
        {
            Confianza = 0.85m + (decimal)Random.Next(0, 15) / 100, // 0.85 - 0.99
            Advertencias = []
        };

        // Detectar tipo de movimiento
        if (lower.Contains("pago") || lower.Contains("gastos") || lower.Contains("egreso"))
            result.Tipo = "Egreso";
        else if (lower.Contains("ingreso") || lower.Contains("transfer") || lower.Contains("pago recibido"))
            result.Tipo = "Ingreso";

        // Detectar categoría basado en palabras clave
        if (lower.Contains("electricidad") || lower.Contains("luz"))
            result.Categoria = "Electricidad";
        else if (lower.Contains("agua"))
            result.Categoria = "Agua";
        else if (lower.Contains("internet"))
            result.Categoria = "Internet";
        else if (lower.Contains("gas"))
            result.Categoria = "Gas";
        else if (lower.Contains("pellet"))
            result.Categoria = "Pellet";
        else if (lower.Contains("mantenimiento"))
            result.Categoria = "Mantenimiento";
        else if (lower.Contains("reparación"))
            result.Categoria = "Reparaciones";
        else if (lower.Contains("aseo") || lower.Contains("limpieza"))
            result.Categoria = "Aseo";
        else if (lower.Contains("gastos comunes"))
            result.Categoria = "Gastos comunes";
        else if (lower.Contains("airbnb"))
        {
            result.Tipo = "Ingreso";
            result.Categoria = lower.Contains("comisión") ? "Comisión Airbnb" : "Arriendo Airbnb";
        }
        else if (lower.Contains("contribución"))
            result.Categoria = "Contribuciones";
        else if (lower.Contains("seguro"))
            result.Categoria = "Seguros";
        else if (lower.Contains("comisión administrador") || lower.Contains("administración"))
            result.Categoria = "Comisión administrador";

        // Extraer monto (busca patrones como "$123.456" o "123456")
        var montoMatch = System.Text.RegularExpressions.Regex.Match(text, @"(?:\$\s*)?(\d{1,3}(?:[,\.]\d{3})*|\d+)");
        if (montoMatch.Success)
        {
            var montoStr = montoMatch.Groups[1].Value.Replace(".", "").Replace(",", "");
            if (decimal.TryParse(montoStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var monto))
                result.Monto = monto;
        }

        // Detectar período (mes/año)
        DetectPeriod(text, out var periodoDesde, out var periodoHasta);
        result.PeriodoDesde = periodoDesde;
        result.PeriodoHasta = periodoHasta;

        // Detectar proveedor
        if (lower.Contains("ultrahost"))
            result.Proveedor = "UltraHost";
        else if (lower.Contains("airbnb"))
            result.Proveedor = "Airbnb";

        // Detectar método de pago
        if (lower.Contains("transferencia"))
            result.MetodoPago = "Transferencia";
        else if (lower.Contains("efectivo"))
            result.MetodoPago = "Efectivo";
        else if (lower.Contains("tarjeta"))
            result.MetodoPago = "Tarjeta";

        // Crear descripción
        result.Descripcion = GenerateDescription(result);

        // Agregar advertencias si falta información
        if (result.Monto == null || result.Monto == 0)
        {
            result.Advertencias.Add("No fue posible detectar el monto con precisión");
            result.Confianza -= 0.2m;
        }
        if (string.IsNullOrEmpty(result.Tipo))
        {
            result.Advertencias.Add("No fue posible determinar si es ingreso o egreso");
            result.Confianza -= 0.3m;
        }

        return result;
    }

    private static void DetectPeriod(string text, out DateOnly? desde, out DateOnly? hasta)
    {
        desde = null;
        hasta = null;

        var lower = text.ToLower();
        var now = DateTime.Now;

        // Buscar nombre de mes
        string[] meses = ["enero", "febrero", "marzo", "abril", "mayo", "junio",
                          "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"];

        for (int i = 0; i < meses.Length; i++)
        {
            if (lower.Contains(meses[i]))
            {
                var mesNum = i + 1;
                desde = new DateOnly(now.Year, mesNum, 1);
                hasta = desde.Value.AddMonths(1).AddDays(-1);
                break;
            }
        }
    }

    private static string GenerateDescription(DetectedTransactionDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Categoria))
            return $"{dto.Categoria}" + (dto.PeriodoDesde.HasValue ? $" del período {dto.PeriodoDesde:MMMM}" : "");

        return "Movimiento financiero";
    }
}
