using Facturacion.Domain.Entities;

namespace Facturacion.Application.Interfaces.Repositories;

public interface IProductoRepository
{
    Task<List<Producto>> ObtenerTodosAsync();

    Task<Producto?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Producto producto);

    Task ActualizarAsync(Producto producto);

    Task GuardarCambiosAsync();

    Task<int> ObtenerProductosActivosAsync();
}