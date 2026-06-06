using Facturacion.Application.DTOs.Pagos;
using Facturacion.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagosController : ControllerBase
{
    private readonly IPagoService _service;

    public PagosController(
        IPagoService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult>
        Registrar(
            RegistrarPagoDto dto)
    {
        await _service
            .RegistrarPagoAsync(dto);

        return Ok(new
        {
            mensaje =
                "Pago registrado correctamente"
        });
    }


    [HttpGet]
    public async Task<IActionResult>
        ObtenerTodos()
    {
        return Ok(
            await _service
                .ObtenerTodosAsync());
    }

    [HttpPut("{id}/anular")]
    public async Task<IActionResult>
        Anular(
            int id)
    {
        await _service
            .AnularPagoAsync(id);

        return Ok(new
        {
            mensaje =
                "Pago anulado correctamente"
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>
        ObtenerPorId(
            int id)
    {
        var pago =
            await _service
                .ObtenerPorIdAsync(id);

        if (pago == null)
            return NotFound();

        return Ok(pago);
    }
}