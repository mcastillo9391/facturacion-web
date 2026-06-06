using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Domain.Entities;
using Facturacion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Repositories;

public class PagoRepository : IPagoRepository
{
    private readonly ApplicationDbContext _context;

    public PagoRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AgregarAsync(
        Pago pago)
    {
        await _context.Pagos
            .AddAsync(pago);
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<decimal>
        ObtenerPagosTotalesAsync()
    {
        return await _context.Pagos
            .SumAsync(x => x.ValorPago);
    }

    public async Task<decimal>
        ObtenerPagosHoyAsync()
    {
        var hoy = DateTime.Today;

        return await _context.Pagos
            .Where(x =>
                x.FechaPago.Date == hoy)
            .Select(x => (decimal?)x.ValorPago)
            .SumAsync() ?? 0;
    }

    public async Task<decimal>
        ObtenerPagosMesActualAsync()
    {
        var hoy = DateTime.Today;

        return await _context.Pagos
            .Where(x =>
                x.FechaPago.Month == hoy.Month &&
                x.FechaPago.Year == hoy.Year)
            .Select(x => (decimal?)x.ValorPago)
            .SumAsync() ?? 0;
    }

    public async Task<List<Pago>>
    ObtenerTodosAsync()
    {
        return await _context.Pagos
            .Include(x => x.Cliente)
            .OrderByDescending(
                x => x.FechaPago)
            .ToListAsync();
    }

    public async Task<Pago?>
        ObtenerPorIdAsync(
            int idPago)
    {
        return await _context.Pagos
            .Include(x => x.Cliente)
            .Include(x => x.DetallesPago)
            .FirstOrDefaultAsync(
                x => x.IdPago == idPago);
    }

}