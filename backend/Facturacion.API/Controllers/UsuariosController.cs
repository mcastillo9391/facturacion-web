using Facturacion.Application.DTOs.Usuarios;
using Facturacion.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador,Vendedor")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(
        IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var usuarios =
            await _usuarioService.ObtenerTodosAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var usuario =
            await _usuarioService.ObtenerPorIdAsync(id);

        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(
        [FromBody] CrearUsuarioDto dto)
    {
        var id =
            await _usuarioService.CrearAsync(dto);

        return Ok(new
        {
            UsuarioId = id
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] ActualizarUsuarioDto dto)
    {
        var actualizado =
            await _usuarioService
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
            await _usuarioService
                .DesactivarAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }

    [HttpGet("roles")]
    public IActionResult ObtenerRoles()
    {
        return Ok(new[]
        {
            new { RolId = 1, Nombre = "Administrador" },
            new { RolId = 2, Nombre = "Cajero" },
            new { RolId = 3, Nombre = "Consulta" }
        });
    }
}