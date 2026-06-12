using Facturacion.Application.DTOs.Facturas;

namespace Facturacion.Application.Interfaces.Services;

public interface IFacturaService
{
    Task<int> GenerarDesdePendienteAsync(
        int pendienteVentaId,
        int usuarioGenerador,
        int? usuarioAsig);

    Task<List<FacturaDto>>
        ObtenerTodasAsync();

    Task<FacturaDetalleDto?>
        ObtenerPorIdAsync(int id);
}