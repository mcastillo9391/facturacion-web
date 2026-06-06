using Facturacion.Domain.Entities;

namespace Facturacion.Application.Interfaces.Repositories;

public interface IPagoRepository
{
    Task AgregarAsync(Pago pago);

    Task GuardarCambiosAsync();

    Task<decimal> ObtenerPagosTotalesAsync();

    Task<decimal> ObtenerPagosHoyAsync();

    Task<decimal> ObtenerPagosMesActualAsync();

    Task<List<Pago>>
    ObtenerTodosAsync();

    Task<Pago?> ObtenerPorIdAsync(
    int idPago);
}