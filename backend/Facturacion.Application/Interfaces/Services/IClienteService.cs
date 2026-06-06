using Facturacion.Application.DTOs.Clientes;

namespace Facturacion.Application.Interfaces.Services;

public interface IClienteService
{
    Task<List<ClienteDto>> ObtenerTodosAsync();

    Task<ClienteDto?> ObtenerPorIdAsync(int id);

    Task<int> CrearAsync(CrearClienteDto dto);

    Task<bool> ActualizarAsync(
        int id,
        ActualizarClienteDto dto);

    Task<bool> DesactivarAsync(int id);
}