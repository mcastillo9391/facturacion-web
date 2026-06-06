using Facturacion.Application.DTOs.PendientesVenta;
using Facturacion.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PendientesVentaController : ControllerBase
{
    private readonly IPendienteVentaService _service;

    public PendientesVentaController(
        IPendienteVentaService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Crear(
        CrearPendienteVentaDto dto)
    {
        int usuarioId = 1;

        var id =
            await _service.CrearAsync(
                dto,
                usuarioId);

        return Ok(new
        {
            PendienteVentaId = id
        });
    }

    [HttpPost("{id}/productos")]
    public async Task<IActionResult> AgregarProducto(
        int id,
        AgregarProductoPendienteDto dto)
    {
        await _service.AgregarProductoAsync(
            id,
            dto);

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Obtener(
        int id)
    {
        var pendiente =
            await _service.ObtenerPorIdAsync(id);

        if (pendiente == null)
            return NotFound();

        return Ok(pendiente);
    }

    [HttpDelete("detalles/{detalleId}")]
    public async Task<IActionResult> EliminarProducto(
        int detalleId)
    {
        try
        {
            await _service.EliminarProductoAsync(detalleId);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var pendientes =
            await _service.ObtenerTodosAsync();

        return Ok(pendientes);
    }

    [HttpPut("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(
        int id)
    {
        await _service.CancelarAsync(id);

        return NoContent();
    }
}