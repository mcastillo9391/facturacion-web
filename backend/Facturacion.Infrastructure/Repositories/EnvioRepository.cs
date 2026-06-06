using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Domain.Entities;
using Facturacion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Repositories;

public class EnvioRepository
    : IEnvioRepository
{
    private readonly ApplicationDbContext _context;

    public EnvioRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Envio>>
        ObtenerTodosAsync()
    {
        return await _context.Envios
            .ToListAsync();
    }

    public async Task<Envio?>
        ObtenerPorIdAsync(int id)
    {
        return await _context.Envios
            .FirstOrDefaultAsync(
                x => x.Codigo == id);
    }

    public async Task AgregarAsync(
        Envio envio)
    {
        await _context.Envios
            .AddAsync(envio);
    }

    public Task ActualizarAsync(
        Envio envio)
    {
        _context.Envios.Update(envio);

        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<decimal>
        ObtenerValorTotalEnviosAsync()
    {
        return await _context.Envios
            .SumAsync(x => x.ValorEnvio);
    }

    public async Task<decimal>
        ObtenerValorEnviosMesActualAsync()
    {
        var hoy = DateTime.Today;

        return await _context.Envios
            .Where(x =>
                x.FechaEnvio.Month == hoy.Month &&
                x.FechaEnvio.Year == hoy.Year)
            .Select(x => (decimal?)x.ValorEnvio)
            .SumAsync() ?? 0;
    }
}