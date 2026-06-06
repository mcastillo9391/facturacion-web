using Facturacion.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarteraController
    : ControllerBase
{
    private readonly ICarteraService
        _service;

    public CarteraController(
        ICarteraService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult>
        Obtener()
    {
        return Ok(
            await _service
                .ObtenerPendientesAsync());
    }
}