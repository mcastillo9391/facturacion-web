using Facturacion.Application.DTOs.Productos;
using Facturacion.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(
        IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var productos =
            await _productoService
                .ObtenerTodosAsync();

        return Ok(productos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(
        int id)
    {
        var producto =
            await _productoService
                .ObtenerPorIdAsync(id);

        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear(
        [FromBody] CrearProductoDto dto)
    {
        var id =
            await _productoService
                .CrearAsync(dto);

        return Ok(new
        {
            IdProducto = id
        });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] ActualizarProductoDto dto)
    {
        var actualizado =
            await _productoService
                .ActualizarAsync(id, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Desactivar(
        int id)
    {
        var eliminado =
            await _productoService
                .DesactivarAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}