using Facturacion.Application.DTOs.Cartera;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;

namespace Facturacion.Application.Services.Recaudo;

public class CarteraService
    : ICarteraService
{
    private readonly IFacturaRepository
        _facturaRepository;

    public CarteraService(
        IFacturaRepository facturaRepository)
    {
        _facturaRepository =
            facturaRepository;
    }

    public async Task<List<CarteraDto>>
        ObtenerPendientesAsync()
    {
        var facturas =
            await _facturaRepository
                .ObtenerFacturasConSaldoPendienteAsync();

        return facturas
            .Select(x =>
                new CarteraDto
                {
                    FacturaId =
                        x.Codigo,

                    Cliente =
                        x.Cliente.Nombre,

                    FechaFactura =
                        x.FechaGen,

                    ValorFactura =
                        x.ValorFactura,

                    ValorAbonado =
                        x.ValorAbonado ?? 0,

                    Saldo =
                        x.ValorFactura
                        - (x.ValorAbonado ?? 0),

                    Estado =
                        x.Estado ?? ""
                })
            .ToList();
    }
}