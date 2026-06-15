namespace Facturacion.Application.DTOs.Chat;
public class ChatResponse
{
    public string Type { get; set; }
        = "text";

    public string Message { get; set; }
        = string.Empty;

    public object? Data { get; set; }
}