using Facturacion.Application.DTOs.Envios;
using Facturacion.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnviosController : ControllerBase
{
    private readonly IEnvioService _envioService;

    public EnviosController(
        IEnvioService envioService)
    {
        _envioService = envioService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var envios =
            await _envioService.ObtenerTodosAsync();

        return Ok(envios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(
        int id)
    {
        var envio =
            await _envioService.ObtenerPorIdAsync(id);

        if (envio == null)
            return NotFound();

        return Ok(envio);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(
        CrearEnvioDto dto)
    {
        await _envioService.CrearAsync(dto);

        return Ok(new
        {
            mensaje = "Envío registrado correctamente"
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarEnvioDto dto)
    {
        await _envioService.ActualizarAsync(
            id,
            dto);

        return Ok(new
        {
            mensaje = "Envío actualizado correctamente"
        });
    }
}