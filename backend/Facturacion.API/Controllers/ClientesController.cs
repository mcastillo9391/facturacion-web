using Facturacion.Application.DTOs.Clientes;
using Facturacion.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(
        IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(
            await _clienteService.ObtenerTodosAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(
        int id)
    {
        var cliente =
            await _clienteService.ObtenerPorIdAsync(id);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(
        CrearClienteDto dto)
    {
        var id =
            await _clienteService.CrearAsync(dto);

        return Ok(new
        {
            IdCli = id
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarClienteDto dto)
    {
        var actualizado =
            await _clienteService
                .ActualizarAsync(id, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desactivar(
        int id)
    {
        var eliminado =
            await _clienteService
                .DesactivarAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}