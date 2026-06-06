using Facturacion.Domain.Entities;

namespace Facturacion.Application.Interfaces.Repositories;

public interface IDetallePendienteVentaRepository
{
    Task<DetallePendienteVenta?>
        ObtenerPorPendienteYProductoAsync(
            int pendienteVentaId,
            int productoId);

    Task<DetallePendienteVenta?>
        ObtenerPorIdAsync(int id);

    Task AgregarAsync(
        DetallePendienteVenta detalle);

    Task EliminarAsync(
        DetallePendienteVenta detalle);

    Task GuardarCambiosAsync();
}