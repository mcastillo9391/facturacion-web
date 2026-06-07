using Facturacion.Application.DTOs.Dashboard;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;

namespace Facturacion.Application.Services.Dashboard;

public class DashboardService
    : IDashboardService
{
    private readonly IFacturaRepository _facturaRepository;

    private readonly IPagoRepository _pagoRepository;

    private readonly IEnvioRepository _envioRepository;

    private readonly IClienteRepository _clienteRepository;

    private readonly IProductoRepository _productoRepository;

    public DashboardService(
        IFacturaRepository facturaRepository,
        IPagoRepository pagoRepository,
        IEnvioRepository envioRepository,
        IClienteRepository clienteRepository,
        IProductoRepository productoRepository)
    {
        _facturaRepository = facturaRepository;
        _pagoRepository = pagoRepository;
        _envioRepository = envioRepository;
        _clienteRepository = clienteRepository;
        _productoRepository = productoRepository;
    }

    public async Task<DashboardDto>
        ObtenerResumenAsync()
    {
        var ventasTotales =
            await _facturaRepository
                .ObtenerVentasTotalesAsync();

        var ventasMes =
            await _facturaRepository
                .ObtenerVentasMesActualAsync();

        var recaudosHoy =
            await _pagoRepository
                .ObtenerPagosHoyAsync();

        var recaudosMes =
            await _pagoRepository
                .ObtenerPagosMesActualAsync();

        var gastosEnvioMes =
            await _envioRepository
                .ObtenerValorEnviosMesActualAsync();

        var clientesActivos =
            await _clienteRepository
                .ObtenerClientesActivosAsync();

        var productosActivos =
            await _productoRepository
                .ObtenerProductosActivosAsync();

        var facturasPendientes =
            await _facturaRepository
                .ObtenerFacturasPendientesAsync();

        var carteraPendiente =
            await _facturaRepository
                .ObtenerCarteraPendienteAsync();

        return new DashboardDto
        {
            
            VentasMes = ventasMes,

            RecaudosHoy = recaudosHoy,

            RecaudosMes = recaudosMes,

            GastosEnvioMes = gastosEnvioMes,

            CarteraPendiente = carteraPendiente,

            FacturasPendientes =
                facturasPendientes,

            ClientesActivos =
                clientesActivos,

            ProductosActivos =
                productosActivos
        };
    }
}