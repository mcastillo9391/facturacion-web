using Facturacion.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController
    : ControllerBase
{
    private readonly IDashboardService
        _dashboardService;

    public DashboardController(
        IDashboardService dashboardService)
    {
        _dashboardService =
            dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult>
        ObtenerResumen()
    {
        var resultado =
            await _dashboardService
                .ObtenerResumenAsync();

        return Ok(resultado);
    }
}