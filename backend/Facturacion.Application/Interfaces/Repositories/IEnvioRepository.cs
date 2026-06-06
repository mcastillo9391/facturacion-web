using Facturacion.Domain.Entities;

namespace Facturacion.Application.Interfaces.Repositories;

public interface IEnvioRepository
{
    Task<List<Envio>> ObtenerTodosAsync();

    Task<Envio?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Envio envio);

    Task ActualizarAsync(Envio envio);

    Task GuardarCambiosAsync();

    Task<decimal> ObtenerValorTotalEnviosAsync();

    Task<decimal> ObtenerValorEnviosMesActualAsync();
}