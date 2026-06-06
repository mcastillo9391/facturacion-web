using Facturacion.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacturasController
    : ControllerBase
{
    private readonly IFacturaService
        _service;

    public FacturasController(
        IFacturaService service)
    {
        _service = service;
    }

    [HttpPost(
        "generar/{pendienteVentaId}")]
    public async Task<IActionResult>
        Generar(
            int pendienteVentaId)
    {
        var facturaId =
            await _service
                .GenerarDesdePendienteAsync(
                    pendienteVentaId);

        return Ok(new
        {
            FacturaId = facturaId
        });
    }


    [HttpGet]
    public async Task<IActionResult>
        ObtenerTodas()
    {
        return Ok(
            await _service
                .ObtenerTodasAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult>
        ObtenerPorId(int id)
    {
        var factura =
            await _service
                .ObtenerPorIdAsync(id);

        if (factura == null)
            return NotFound();

        return Ok(factura);
    }
}