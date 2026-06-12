using Facturacion.Application.DTOs.Cartera;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;

namespace Facturacion.Application.Services.Recaudo;

public class CarteraService
    : ICarteraService
{
    private readonly IFacturaRepository
        _facturaRepository;
    
    private readonly IUsuarioActualService _usuarioActual;

    
    public CarteraService(
        IFacturaRepository facturaRepository,
        IUsuarioActualService usuarioActual)
    {
        _facturaRepository = facturaRepository;
        _usuarioActual = usuarioActual;
    }

    public async Task<List<CarteraDto>>
        ObtenerPendientesAsync()
    {
        var facturas =
            await _facturaRepository
                .ObtenerFacturasConSaldoPendienteAsync();

        if ((_usuarioActual.Rol ?? "")
            .Trim()
            .ToUpper() == "CONSULTA")
        {
            facturas = facturas
                .Where(x =>
                    x.UsuarioAsig ==
                    _usuarioActual.UsuarioId)
                .ToList();
        }
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