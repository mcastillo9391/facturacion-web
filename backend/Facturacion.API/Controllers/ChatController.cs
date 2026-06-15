using Facturacion.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(
        IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> Ask(
        [FromBody] ChatRequest request)
    {
        var response =
            await _chatService
                .AskAsync(
                    request.Message);

        return Ok(response);
    }
}

public class ChatRequest
{
    public string Message { get; set; }
        = string.Empty;
}