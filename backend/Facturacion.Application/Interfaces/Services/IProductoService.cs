using Facturacion.Application.DTOs.Productos;

namespace Facturacion.Application.Interfaces.Services;

public interface IProductoService
{
    Task<List<ProductoDto>> ObtenerTodosAsync();

    Task<ProductoDto?> ObtenerPorIdAsync(int id);

    Task<int> CrearAsync(CrearProductoDto dto);

    Task<bool> ActualizarAsync(
        int id,
        ActualizarProductoDto dto);

    Task<bool> DesactivarAsync(int id);
}