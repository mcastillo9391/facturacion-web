using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;
using Facturacion.Domain.Entities;
using Facturacion.Application.DTOs.Facturas;


namespace Facturacion.Application.Services.Ventas;

public class FacturaService : IFacturaService
{
    private readonly IPendienteVentaRepository
        _pendienteRepository;

    private readonly IFacturaRepository
        _facturaRepository;

    private readonly IUsuarioActualService _usuarioActual;
    
    public FacturaService(
        IPendienteVentaRepository pendienteRepository,
        IFacturaRepository facturaRepository,
        IUsuarioActualService usuarioActual)
    {
        _pendienteRepository = pendienteRepository;
        _facturaRepository = facturaRepository;
        _usuarioActual = usuarioActual;
    }

    public async Task<int>
        GenerarDesdePendienteAsync(
            int pendienteVentaId,
            int usuarioGenerador,
            int? usuarioAsigDto)
    {
        var pendiente =
            await _pendienteRepository
                .ObtenerPorIdAsync(
                    pendienteVentaId);

        if (pendiente == null)
        {
            throw new Exception(
                "Pendiente de venta no encontrado");
        }

        if (pendiente.Estado != "ABIERTO")
        {
            throw new Exception(
                $"La venta se encuentra en estado {pendiente.Estado} y no puede ser facturada.");
        }

        if (pendiente.Detalles.Count == 0)
        {
            throw new Exception(
                "No existen productos asociados a la venta.");
        }

        if (!pendiente.Cliente.Activo)
        {
            throw new Exception(
                "No se puede facturar un cliente inactivo.");
        }

        // Prioridad: 1) el que viene en el DTO (seleccionado en el modal)
        //            2) el guardado en el pendiente
        //            3) el usuario generador como fallback
        var usuarioAsig =
            usuarioAsigDto
            ?? pendiente.UsuarioAsig
            ?? usuarioGenerador;

        var factura =
            new Factura
            {
                IdCli = pendiente.ClienteId,
                UsuarioId = pendiente.UsuarioId,
                UsuarioAsig = usuarioAsig,
                FechaGen = DateTime.Now,
                Estado = "PENDIENTE",
                ValorAbonado = 0,
                ValorFactura =
                    pendiente.Detalles.Sum(
                        x => x.Total)
            };

        foreach (var detalle in pendiente.Detalles)
        {
            if (!detalle.Producto.Activo)
            {
                throw new Exception(
                    $"El producto {detalle.Producto.Nombre} está inactivo.");
            }
        }

        foreach (var detalle in pendiente.Detalles)
        {
            factura.Detalles.Add(
                new DetalleFactura
                {
                    Producto =
                        detalle.ProductoId,

                    CostoUnitario =
                        (float)detalle
                            .Producto
                            .ValorCosto,

                    ValorUnitario =
                        (float)detalle
                            .ValorUnitario,

                    Descuento =
                        (float)detalle
                            .Descuento,

                    Cantidad =
                        detalle.Cantidad,

                    Total =
                        (float)detalle.Total
                });
        }

        await _facturaRepository
            .AgregarAsync(factura);

        pendiente.Estado =
            "FACTURADA";

        await _pendienteRepository
            .ActualizarAsync(
                pendiente);

        await _facturaRepository
            .GuardarCambiosAsync();

        return factura.Codigo;
    }

    public async Task<List<FacturaDto>> ObtenerTodasAsync()
    {
        var facturas =
            await _facturaRepository
                .ObtenerTodasAsync();

        if ((_usuarioActual.Rol ?? "")
            .Trim()
            .ToUpper() == "CONSULTA")
        {
            facturas = facturas
                .Where(f =>
                    f.UsuarioAsig ==
                    _usuarioActual.UsuarioId)
                .ToList();

            Console.WriteLine(
                $"Facturas después del filtro: {facturas.Count}");
        }

        return facturas
            .Select(f => new FacturaDto
            {              
                Codigo = f.Codigo,
                Cliente = f.Cliente.Nombre,

                UsuarioId = f.UsuarioId ?? 0,
                UsuarioAsig = f.UsuarioAsig,

                FechaGen = f.FechaGen,
                ValorFactura = f.ValorFactura,
                ValorAbonado = f.ValorAbonado ?? 0,
                Estado = f.Estado ?? ""
            })
            .ToList();
    }

    public async Task<FacturaDetalleDto?>
        ObtenerPorIdAsync(int id)
    {
        var factura =
            await _facturaRepository
                .ObtenerPorIdAsync(id);

        if (factura == null)
            return null;

        if (
            factura != null
            && _usuarioActual.Rol == "CONSULTA"
            && factura.UsuarioAsig != _usuarioActual.UsuarioId
        )
        {
            return null;
        }
        return new FacturaDetalleDto
        {
            Codigo =
                factura.Codigo,

            Cliente =
                factura.Cliente.Nombre,

            FechaGen =
                factura.FechaGen,

            ValorFactura =
                factura.ValorFactura,

            ValorAbonado =
                factura.ValorAbonado ?? 0,

            Estado =
                factura.Estado ?? "",

            Detalles =
                factura.Detalles
                    .Select(d =>
                        new DetalleFacturaDto
                        {
                            Codigo =
                                d.Codigo,

                            Producto =
                                d.ProductoNavigation.Nombre,

                            Cantidad =
                                d.Cantidad,

                            ValorUnitario =
                                (decimal)d.ValorUnitario,

                            Descuento =
                                (decimal)(d.Descuento ?? 0),

                            Total =
                                (decimal)d.Total
                        })
                    .ToList()
        };
    }
}
