using Facturacion.Application.DTOs.Productos;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;
using Facturacion.Domain.Entities;

namespace Facturacion.Application.Services.Productos;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(
        IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<List<ProductoDto>> ObtenerTodosAsync()
    {
        var productos =
            await _productoRepository
                .ObtenerTodosAsync();

        return productos
            .Select(x => new ProductoDto
            {
                IdProducto = x.IdProducto,
                Nombre = x.Nombre,
                ValorCosto = x.ValorCosto,
                ValorVenta = x.ValorVenta,
                Activo = x.Activo
            })
            .ToList();
    }

    public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
    {
        var producto =
            await _productoRepository
                .ObtenerPorIdAsync(id);

        if (producto == null)
            return null;

        return new ProductoDto
        {
            IdProducto = producto.IdProducto,
            Nombre = producto.Nombre,
            ValorCosto = producto.ValorCosto,
            ValorVenta = producto.ValorVenta,
            Activo = producto.Activo
        };
    }

    public async Task<int> CrearAsync(
        CrearProductoDto dto)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre,
            ValorCosto = dto.ValorCosto,
            ValorVenta = dto.ValorVenta,
            Activo = true
        };

        await _productoRepository
            .AgregarAsync(producto);

        await _productoRepository
            .GuardarCambiosAsync();

        return producto.IdProducto;
    }

    public async Task<bool> ActualizarAsync(
        int id,
        ActualizarProductoDto dto)
    {
        var producto =
            await _productoRepository
                .ObtenerPorIdAsync(id);

        if (producto == null)
            return false;

        producto.Nombre = dto.Nombre;
        producto.ValorCosto = dto.ValorCosto;
        producto.ValorVenta = dto.ValorVenta;
        producto.Activo = dto.Activo;

        await _productoRepository
            .ActualizarAsync(producto);

        await _productoRepository
            .GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> DesactivarAsync(
        int id)
    {
        var producto =
            await _productoRepository
                .ObtenerPorIdAsync(id);

        if (producto == null)
            return false;

        producto.Activo = false;

        await _productoRepository
            .ActualizarAsync(producto);

        await _productoRepository
            .GuardarCambiosAsync();

        return true;
    }
}