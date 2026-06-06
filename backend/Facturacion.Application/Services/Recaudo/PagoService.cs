using Facturacion.Application.DTOs.Pagos;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;
using Facturacion.Domain.Entities;

namespace Facturacion.Application.Services.Recaudo;

public class PagoService : IPagoService
{
    private readonly IFacturaRepository
        _facturaRepository;

    private readonly IPagoRepository
        _pagoRepository;

    public PagoService(
        IFacturaRepository facturaRepository,
        IPagoRepository pagoRepository)
    {
        _facturaRepository = facturaRepository;
        _pagoRepository = pagoRepository;
    }

    public async Task RegistrarPagoAsync(
        RegistrarPagoDto dto)
    {
        var factura =
            await _facturaRepository
                .ObtenerPorIdAsync(dto.FacturaId);

        if (factura == null)
            throw new Exception(
                "Factura no encontrada");

        var saldo =
            factura.ValorFactura
            - (factura.ValorAbonado ?? 0);

        if (dto.ValorPago > saldo)
        {
            throw new Exception(
                $"El pago supera el saldo pendiente ({saldo}).");
        }

        var pago = new Pago
        {
            IdCli = factura.IdCli,
            FechaPago = DateTime.Now,
            ValorPago = dto.ValorPago
        };

        pago.DetallesPago.Add(
            new DetallePago
            {
                Factura = factura.Codigo,
                ValorPago = dto.ValorPago
            });

        await _pagoRepository
            .AgregarAsync(pago);

        factura.ValorAbonado =
            (factura.ValorAbonado ?? 0)
            + dto.ValorPago;

        if (factura.ValorAbonado
            >= factura.ValorFactura)
        {
            factura.Estado = "PAGADA";
        }
        else
        {
            factura.Estado = "PENDIENTE";
        }

        await _facturaRepository
            .ActualizarAsync(factura);

        await _pagoRepository
            .GuardarCambiosAsync();
    }

    public async Task<List<PagoResponseDto>>
        ObtenerTodosAsync()
    {
        var pagos =
            await _pagoRepository
                .ObtenerTodosAsync();

        return pagos
            .Select(x =>
                new PagoResponseDto
                {
                    IdPago = x.IdPago,
                    IdCli = x.IdCli,
                    Cliente = x.Cliente.Nombre,
                    FechaPago = x.FechaPago,
                    FechaAnulacion = x.FechaAnulacion,
                    ValorPago = x.ValorPago
                })
            .ToList();
    }

    public async Task AnularPagoAsync(int idPago)
    {
        var pago =
            await _pagoRepository
                .ObtenerPorIdAsync(idPago);

        if (pago == null)
            throw new Exception(
                "Pago no encontrado");

        if (pago.FechaAnulacion != null)
            throw new Exception(
                "El pago ya fue anulado");

        foreach (
            var detalle
            in pago.DetallesPago)
        {
            var factura =
                await _facturaRepository
                    .ObtenerPorIdAsync(
                        detalle.Factura);

            if (factura != null)
            {
                factura.ValorAbonado =
                    (factura.ValorAbonado ?? 0)
                    - detalle.ValorPago;

                if (
                    factura.ValorAbonado
                    < factura.ValorFactura
                )
                {
                    factura.Estado =
                        "PENDIENTE";
                }

                await _facturaRepository
                    .ActualizarAsync(
                        factura);
            }
        }

        pago.FechaAnulacion =
            DateTime.Now;

        await _pagoRepository
            .GuardarCambiosAsync();
    }

    public async Task<PagoResponseDto?>
        ObtenerPorIdAsync(
            int idPago)
    {
        var pago =
            await _pagoRepository
                .ObtenerPorIdAsync(
                    idPago);

        if (pago == null)
            return null;

        return new PagoResponseDto
        {
            IdPago = pago.IdPago,
            IdCli = pago.IdCli,
            Cliente = pago.Cliente.Nombre,
            FechaPago = pago.FechaPago,
            FechaAnulacion = pago.FechaAnulacion,
            ValorPago = pago.ValorPago
        };
    }
}