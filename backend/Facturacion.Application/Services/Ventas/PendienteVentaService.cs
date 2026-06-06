using Facturacion.Application.DTOs.PendientesVenta;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;
using Facturacion.Domain.Entities;

namespace Facturacion.Application.Services.Ventas;

public class PendienteVentaService : IPendienteVentaService
{
    private readonly IPendienteVentaRepository
        _pendienteVentaRepository;

    private readonly IDetallePendienteVentaRepository
        _detalleRepository;

    private readonly IProductoRepository
        _productoRepository;

    private readonly IClienteRepository
        _clienteRepository;

    public PendienteVentaService(
        IPendienteVentaRepository pendienteVentaRepository,
        IDetallePendienteVentaRepository detalleRepository,
        IProductoRepository productoRepository,
        IClienteRepository clienteRepository)
    {
        _pendienteVentaRepository =
            pendienteVentaRepository;

        _detalleRepository =
            detalleRepository;

        _productoRepository =
            productoRepository;

        _clienteRepository =
            clienteRepository;
    }

    public async Task<int> CrearAsync(
        CrearPendienteVentaDto dto,
        int usuarioId)
    {
        var cliente =
            await _clienteRepository
                .ObtenerPorIdAsync(dto.ClienteId);

        if (cliente == null)
        {
            throw new Exception(
                "Cliente no existe.");
        }

        if (!cliente.Activo)
        {
            throw new Exception(
                "No se puede crear una venta para un cliente inactivo.");
        }

        var pendiente =
            new PendienteVenta
            {
                ClienteId = dto.ClienteId,
                UsuarioId = usuarioId,
                FechaCreacion = DateTime.Now,
                Estado = "ABIERTO",
                Observaciones = dto.Observaciones
            };

        await _pendienteVentaRepository
            .AgregarAsync(pendiente);

        await _pendienteVentaRepository
            .GuardarCambiosAsync();

        return pendiente.PendienteVentaId;
    }

    public async Task AgregarProductoAsync(
        int pendienteVentaId,
        AgregarProductoPendienteDto dto)
    {
        var pendiente =
            await _pendienteVentaRepository
                .ObtenerPorIdAsync(
                    pendienteVentaId);

        if (pendiente == null)
            throw new Exception(
                "Pendiente de venta no existe.");

        if (pendiente.Estado != "ABIERTO")
        {
            throw new Exception(
                "Solo se pueden modificar ventas abiertas.");
        }

        var producto =
            await _productoRepository
                .ObtenerPorIdAsync(
                    dto.ProductoId);

        if (producto == null)
        {
            throw new Exception(
                "Producto no existe.");
        }

        if (!producto.Activo)
        {
            throw new Exception(
                "No se puede agregar un producto inactivo.");
        }

        var detalleExistente =
            await _detalleRepository
                .ObtenerPorPendienteYProductoAsync(
                    pendienteVentaId,
                    dto.ProductoId);

        if (detalleExistente != null)
        {
            detalleExistente.Cantidad +=
                dto.Cantidad;

            detalleExistente.Total =
                (detalleExistente.Cantidad *
                 detalleExistente.ValorUnitario)
                - detalleExistente.Descuento;

            await _detalleRepository
                .GuardarCambiosAsync();

            return;
        }

        var detalle =
            new DetallePendienteVenta
            {
                PendienteVentaId =
                    pendienteVentaId,

                ProductoId =
                    dto.ProductoId,

                Cantidad =
                    dto.Cantidad,

                ValorUnitario =
                    producto.ValorVenta,

                Descuento =
                    dto.Descuento,

                Total =
                    (dto.Cantidad *
                     producto.ValorVenta)
                    - dto.Descuento
            };

        await _detalleRepository
            .AgregarAsync(detalle);

        await _detalleRepository
            .GuardarCambiosAsync();
    }

    public async Task<PendienteVentaDetalleDto?>
        ObtenerPorIdAsync(int id)
    {
        var pendiente =
            await _pendienteVentaRepository
                .ObtenerPorIdAsync(id);

        if (pendiente == null)
            return null;

        return new PendienteVentaDetalleDto
        {
            PendienteVentaId =
                pendiente.PendienteVentaId,

            ClienteId =
                pendiente.ClienteId,

            Cliente =
                pendiente.Cliente.Nombre,

            Estado =
                pendiente.Estado,

            Total =
                pendiente.Detalles
                    .Sum(x => x.Total),

            Detalles =
                pendiente.Detalles
                    .Select(x =>
                        new DetallePendienteVentaDto
                        {
                            DetallePendienteVentaId =
                                x.DetallePendienteVentaId,

                            ProductoId =
                                x.ProductoId,

                            Producto =
                                x.Producto.Nombre,

                            Cantidad =
                                x.Cantidad,

                            ValorUnitario =
                                x.ValorUnitario,

                            Descuento =
                                x.Descuento,

                            Total =
                                x.Total
                        })
                    .ToList()
        };
    }

    public async Task EliminarProductoAsync(
        int detalleId)
    {
        
        // Revalidar el detalle después de obtener el pendiente para asegurarse de que existe y pertenece al pendiente
        var detalle =
            await _detalleRepository
                .ObtenerPorIdAsync(detalleId);
                
        // Validar que el detalle exista y obtener el pendiente para validar su estado  
        var pendiente =
            await _pendienteVentaRepository
                .ObtenerPorIdAsync(
                    detalle.PendienteVentaId);
        


        // Validar que el pendiente exista
        if (pendiente == null)
        {
            throw new Exception(
                "Pendiente no encontrado.");
        }

        // Solo se pueden modificar pendientes abiertos
        if (pendiente.Estado != "ABIERTO")
        {
            throw new Exception(
                "Solo se pueden modificar ventas abiertas.");
        }

        if (detalle == null)
            throw new Exception(
                "Detalle no encontrado.");

        await _detalleRepository
            .EliminarAsync(detalle);

        await _detalleRepository
            .GuardarCambiosAsync();
    }

    public async Task<List<PendienteVentaDto>>
        ObtenerTodosAsync()
    {
        var pendientes =
            await _pendienteVentaRepository
                .ObtenerTodosAsync();

        return pendientes
            .Select(x =>
                new PendienteVentaDto
                {
                    PendienteVentaId =
                        x.PendienteVentaId,

                    ClienteId =
                        x.ClienteId,

                    Cliente =
                        x.Cliente.Nombre,

                    Estado =
                        x.Estado,

                    Total =
                        x.Detalles.Sum(
                            d => d.Total),

                    Detalles =
                        x.Detalles
                            .Select(d =>
                                new DetallePendienteVentaDto
                                {
                                    DetallePendienteVentaId =
                                        d.DetallePendienteVentaId,

                                    ProductoId =
                                        d.ProductoId,

                                    Producto =
                                        d.Producto.Nombre,

                                    Cantidad =
                                        d.Cantidad,

                                    ValorUnitario =
                                        d.ValorUnitario,

                                    Descuento =
                                        d.Descuento,

                                    Total =
                                        d.Total
                                })
                            .ToList()
                })
            .ToList();
    }

    public async Task
        CancelarAsync(int id)
    {
        var pendiente =
            await _pendienteVentaRepository
                .ObtenerPorIdAsync(id);

        if (pendiente == null)
            throw new Exception(
                "Pendiente no encontrado");

        if (pendiente.Estado ==
            "FACTURADA")
        {
            throw new Exception(
                "No se puede cancelar una venta facturada");
        }

        pendiente.Estado =
            "CANCELADA";

        await _pendienteVentaRepository
            .GuardarCambiosAsync();
    }

}