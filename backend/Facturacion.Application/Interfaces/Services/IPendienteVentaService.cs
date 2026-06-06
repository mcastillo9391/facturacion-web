using Facturacion.Application.DTOs.PendientesVenta;

namespace Facturacion.Application.Interfaces.Services;

public interface IPendienteVentaService
{
    Task<int> CrearAsync(
        CrearPendienteVentaDto dto,
        int usuarioId);

    Task AgregarProductoAsync(
        int pendienteVentaId,
        AgregarProductoPendienteDto dto);

    Task<PendienteVentaDetalleDto?>
        ObtenerPorIdAsync(int id);

    Task EliminarProductoAsync(
        int detalleId);

    Task<List<PendienteVentaDto>>
    ObtenerTodosAsync();

    Task CancelarAsync(int id);

}