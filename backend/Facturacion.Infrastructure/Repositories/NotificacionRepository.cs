using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Domain.Entities;
using Facturacion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Repositories;

public class NotificacionRepository
    : INotificacionRepository
{
    private readonly
        ApplicationDbContext _context;

    public NotificacionRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CrearAsync(
        Notificacion notificacion)
    {
        await _context.Notificaciones
            .AddAsync(notificacion);
    }

    public async Task<List<Notificacion>>
        ObtenerPorUsuarioAsync(
            int usuarioId)
    {
        return await _context.Notificaciones
            .Where(
                x => x.UsuarioId == usuarioId)
            .OrderByDescending(
                x => x.Fecha)
            .ToListAsync();
    }

    public async Task<Notificacion?>
        ObtenerPorIdAsync(
            int id)
    {
        return await _context.Notificaciones
            .FirstOrDefaultAsync(
                x => x.Id == id);
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}