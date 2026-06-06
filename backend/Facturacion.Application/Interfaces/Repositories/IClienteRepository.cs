using Facturacion.Domain.Entities;

namespace Facturacion.Application.Interfaces.Repositories;

public interface IClienteRepository
{
    Task<List<Cliente>> ObtenerTodosAsync();

    Task<Cliente?> ObtenerPorIdAsync(int id);

    Task<Cliente?> ObtenerPorIdentificacionAsync(
        string identificacion);

    Task AgregarAsync(Cliente cliente);

    Task ActualizarAsync(Cliente cliente);

    Task GuardarCambiosAsync();

    Task<int> ObtenerClientesActivosAsync();
}