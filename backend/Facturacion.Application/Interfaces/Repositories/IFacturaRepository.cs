using Facturacion.Domain.Entities;

namespace Facturacion.Application.Interfaces.Repositories;

public interface IFacturaRepository
{
    Task<Factura?> ObtenerPorIdAsync(int id);

    Task<List<Factura>> ObtenerTodasAsync();

    Task AgregarAsync(Factura factura);

    Task ActualizarAsync(Factura factura);

    Task GuardarCambiosAsync();

    Task<decimal> ObtenerVentasTotalesAsync();

    Task<decimal> ObtenerVentasMesActualAsync();

    Task<int> ObtenerCantidadFacturasAsync();

    Task<int> ObtenerFacturasPendientesAsync();
    
    Task<List<Factura>> ObtenerPorUsuarioAsigAsync(
    int usuarioId);
    
    Task<List<Factura>>
    ObtenerFacturasConSaldoPendienteAsync();

    Task<decimal> ObtenerCarteraPendienteAsync();
    
}