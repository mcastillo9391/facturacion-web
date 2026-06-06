using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Domain.Entities;
using Facturacion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorUsernameAsync(
        string username)
    {
        return await _context.Usuarios
            .Include(x => x.Rol)
            .FirstOrDefaultAsync(
                x => x.Username == username);
    }

    public async Task<List<Usuario>> ObtenerTodosAsync()
    {
        return await _context.Usuarios
            .Include(x => x.Rol)
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        return await _context.Usuarios
            .Include(x => x.Rol)
            .FirstOrDefaultAsync(
                x => x.UsuarioId == id);
    }

    public async Task AgregarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task ActualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);

        return Task.CompletedTask;
    }
}