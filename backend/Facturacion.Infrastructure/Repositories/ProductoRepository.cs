using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Domain.Entities;
using Facturacion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly ApplicationDbContext _context;

    public ProductoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producto>> ObtenerTodosAsync()
    {
        return await _context.Productos
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Productos
            .FirstOrDefaultAsync(x =>
                x.IdProducto == id);
    }

    public async Task AgregarAsync(
        Producto producto)
    {
        await _context.Productos
            .AddAsync(producto);
    }

    public Task ActualizarAsync(
        Producto producto)
    {
        _context.Productos.Update(producto);

        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<int>
        ObtenerProductosActivosAsync()
    {
        return await _context.Productos
            .CountAsync(x => x.Activo);
    }
}