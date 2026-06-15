using Microsoft.AspNetCore.Mvc;
using Facturacion.Application.Interfaces.Services;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificacionesController : ControllerBase
{
    private readonly INotificacionService _service;

    public NotificacionesController(
        INotificacionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult>
        ObtenerMisNotificaciones()
    {
        var resultado =
            await _service
                .ObtenerMisNotificacionesAsync();

        return Ok(resultado);
    }

    [HttpPut("{id}/leer")]
    public async Task<IActionResult>
        MarcarLeida(int id)
    {
        await _service
            .MarcarLeidaAsync(id);

        return NoContent();
    }
}