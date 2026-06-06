using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Domain.Entities;
using Facturacion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Repositories;

public class PendienteVentaRepository
    : IPendienteVentaRepository
{
    private readonly ApplicationDbContext _context;

    public PendienteVentaRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PendienteVenta>>
        ObtenerTodosAsync()
    {
        return await _context
            .PendientesVenta
            .Include(x => x.Cliente)
            .Include(x => x.Usuario)
            .Include(x => x.Detalles)
                .ThenInclude(x => x.Producto)
            .OrderByDescending(
                x => x.FechaCreacion)
            .ToListAsync();
    }

    public async Task<PendienteVenta?>
        ObtenerPorIdAsync(int id)
    {
        return await _context
            .PendientesVenta
            .Include(x => x.Cliente)
            .Include(x => x.Usuario)
            .Include(x => x.Detalles)
                .ThenInclude(x => x.Producto)
            .FirstOrDefaultAsync(
                x => x.PendienteVentaId == id);
    }

    public async Task AgregarAsync(
        PendienteVenta pendienteVenta)
    {
        await _context
            .PendientesVenta
            .AddAsync(pendienteVenta);
    }

    public Task ActualizarAsync(
        PendienteVenta pendienteVenta)
    {
        _context
            .PendientesVenta
            .Update(pendienteVenta);

        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context
            .SaveChangesAsync();
    }
}