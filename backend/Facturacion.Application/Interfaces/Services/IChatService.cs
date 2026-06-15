using Facturacion.Application.DTOs.Chat;
namespace Facturacion.Application.Interfaces.Services;

public interface IChatService
{
    Task<ChatResponse> AskAsync(string message);
}