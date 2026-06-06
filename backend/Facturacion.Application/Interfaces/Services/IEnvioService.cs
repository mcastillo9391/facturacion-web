using Facturacion.Application.DTOs.Envios;

namespace Facturacion.Application.Interfaces.Services;

public interface IEnvioService
{
    Task<List<EnvioResponseDto>> ObtenerTodosAsync();

    Task<EnvioResponseDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(CrearEnvioDto dto);

    Task ActualizarAsync(
        int id,
        ActualizarEnvioDto dto);
}