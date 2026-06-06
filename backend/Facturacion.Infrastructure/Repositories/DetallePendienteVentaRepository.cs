using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Domain.Entities;
using Facturacion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Repositories;

public class DetallePendienteVentaRepository
    : IDetallePendienteVentaRepository
{
    private readonly ApplicationDbContext _context;

    public DetallePendienteVentaRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DetallePendienteVenta?>
        ObtenerPorPendienteYProductoAsync(
            int pendienteVentaId,
            int productoId)
    {
        return await _context.DetallesPendienteVenta
            .FirstOrDefaultAsync(x =>
                x.PendienteVentaId == pendienteVentaId
                && x.ProductoId == productoId);
    }

    public async Task<DetallePendienteVenta?>
        ObtenerPorIdAsync(int id)
    {
        return await _context.DetallesPendienteVenta
            .FirstOrDefaultAsync(
                x => x.DetallePendienteVentaId == id);
    }

    public async Task AgregarAsync(
        DetallePendienteVenta detalle)
    {
        await _context.DetallesPendienteVenta
            .AddAsync(detalle);
    }

    public Task EliminarAsync(
        DetallePendienteVenta detalle)
    {
        _context.DetallesPendienteVenta
            .Remove(detalle);

        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}