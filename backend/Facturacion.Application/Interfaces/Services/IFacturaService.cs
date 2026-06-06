using Facturacion.Application.DTOs.Facturas;

namespace Facturacion.Application.Interfaces.Services;

public interface IFacturaService
{
    Task<int> GenerarDesdePendienteAsync(
        int pendienteVentaId);

    Task<List<FacturaDto>>
        ObtenerTodasAsync();

    Task<FacturaDetalleDto?>
        ObtenerPorIdAsync(int id);
}