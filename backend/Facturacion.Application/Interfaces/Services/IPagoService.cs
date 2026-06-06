using Facturacion.Application.DTOs.Pagos;

namespace Facturacion.Application.Interfaces.Services;

public interface IPagoService
{
    Task RegistrarPagoAsync(
        RegistrarPagoDto dto);

    Task<List<PagoResponseDto>>
        ObtenerTodosAsync();

    Task<PagoResponseDto?>
        ObtenerPorIdAsync(
            int idPago);

    Task AnularPagoAsync(int idPago);
}