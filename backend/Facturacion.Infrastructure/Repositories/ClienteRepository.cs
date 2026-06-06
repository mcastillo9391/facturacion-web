using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Domain.Entities;
using Facturacion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ApplicationDbContext _context;

    public ClienteRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cliente>> ObtenerTodosAsync()
    {
        return await _context.Clientes
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }

    public async Task<Cliente?> ObtenerPorIdAsync(
        int id)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(x =>
                x.IdCli == id);
    }

    public async Task<Cliente?> ObtenerPorIdentificacionAsync(
        string identificacion)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(x =>
                x.Identificacion == identificacion);
    }

    public async Task AgregarAsync(
        Cliente cliente)
    {
        await _context.Clientes
            .AddAsync(cliente);
    }

    public Task ActualizarAsync(
        Cliente cliente)
    {
        _context.Clientes.Update(cliente);

        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<int>
        ObtenerClientesActivosAsync()
    {
        return await _context.Clientes
            .CountAsync(x => x.Activo);
    }
}