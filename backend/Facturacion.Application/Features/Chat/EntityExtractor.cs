namespace Facturacion.Application.Features.Chat;

public static class EntityExtractor
{
    public static string LimpiarPregunta(
        string text)
    {
        return text
            .ToLowerInvariant()
            .Replace("¿", "")
            .Replace("?", "")
            .Replace("cuánto debe", "")
            .Replace("cuanto debe", "")
            .Replace("cliente", "")
            .Replace("estado de", "")
            .Replace("estado cliente", "")
            .Replace("tiene deuda", "")
            .Replace("saldo de", "")
            .Trim();
    }
}