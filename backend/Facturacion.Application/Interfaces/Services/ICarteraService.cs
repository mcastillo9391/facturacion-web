using Facturacion.Application.DTOs.Cartera;

namespace Facturacion.Application.Interfaces.Services;

public interface ICarteraService
{
    Task<List<CarteraDto>>
        ObtenerPendientesAsync();
}