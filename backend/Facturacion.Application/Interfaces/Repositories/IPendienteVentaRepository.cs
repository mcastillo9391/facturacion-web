using Facturacion.Domain.Entities;

namespace Facturacion.Application.Interfaces.Repositories;

public interface IPendienteVentaRepository
{
    Task<List<PendienteVenta>>
        ObtenerTodosAsync();

    Task<PendienteVenta?>
        ObtenerPorIdAsync(int id);

    Task AgregarAsync(
        PendienteVenta pendienteVenta);

    Task ActualizarAsync(
        PendienteVenta pendienteVenta);

    Task GuardarCambiosAsync();
}