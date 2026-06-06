using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Domain.Entities;
using Facturacion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Repositories;

public class FacturaRepository : IFacturaRepository
{
    private readonly ApplicationDbContext _context;

    public FacturaRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Factura?> ObtenerPorIdAsync(
        int id)
    {
        return await _context.Facturas
            .Include(x => x.Cliente)
            .Include(x => x.Usuario)
            .Include(x => x.Detalles)
                .ThenInclude(x => x.ProductoNavigation)
            .FirstOrDefaultAsync(
                x => x.Codigo == id);
    }

    public async Task<List<Factura>>
        ObtenerTodasAsync()
    {
        return await _context.Facturas
            .Include(x => x.Cliente)
            .ToListAsync();
    }

    public async Task AgregarAsync(
        Factura factura)
    {
        await _context.Facturas
            .AddAsync(factura);
    }

    public Task ActualizarAsync(
        Factura factura)
    {
        _context.Facturas.Update(factura);

        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<decimal>
        ObtenerVentasTotalesAsync()
    {
        return await _context.Facturas
            .SumAsync(x => x.ValorFactura);
    }

    public async Task<decimal>
        ObtenerVentasMesActualAsync()
    {
        var hoy = DateTime.Today;

        return await _context.Facturas
            .Where(x =>
                x.FechaGen.Month == hoy.Month &&
                x.FechaGen.Year == hoy.Year)
            .SumAsync(x => x.ValorFactura);
    }

    public async Task<int>
        ObtenerCantidadFacturasAsync()
    {
        return await _context.Facturas.CountAsync();
    }

    public async Task<int>
        ObtenerFacturasPendientesAsync()
    {
        return await _context.Facturas
            .CountAsync(x =>
                x.Estado != null &&
                x.Estado.ToUpper() == "PENDIENTE");
    }

    public async Task<List<Factura>>
        ObtenerFacturasConSaldoPendienteAsync()
    {
        return await _context.Facturas
            .Include(x => x.Cliente)
            .Where(x =>
                x.ValorFactura >
                (x.ValorAbonado ?? 0))
            .OrderByDescending(
                x => x.FechaGen)
            .ToListAsync();
    }

    public async Task<decimal>
        ObtenerCarteraPendienteAsync()
    {
        return await _context.Facturas
            .Where(x =>
                x.ValorFactura >
                (x.ValorAbonado ?? 0))
            .SumAsync(x =>
                x.ValorFactura
                - (x.ValorAbonado ?? 0));
    }
}