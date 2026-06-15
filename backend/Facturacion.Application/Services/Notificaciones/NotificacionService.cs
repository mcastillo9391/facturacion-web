using Facturacion.Application.DTOs.Notificaciones;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;
using Facturacion.Domain.Entities;

namespace Facturacion.Application.Services.Notificaciones;

public class NotificacionService
    : INotificacionService
{
    private readonly
        INotificacionRepository
        _repository;

    private readonly
        IUsuarioActualService
        _usuarioActual;

    public NotificacionService(
        INotificacionRepository repository,
        IUsuarioActualService usuarioActual)
    {
        _repository = repository;
        _usuarioActual = usuarioActual;
    }

    public async Task CrearAsync(
        int usuarioId,
        string titulo,
        string mensaje)
    {
        await _repository.CrearAsync(
            new Notificacion
            {
                UsuarioId = usuarioId,
                Titulo = titulo,
                Mensaje = mensaje,
                Leida = false,
                Fecha = DateTime.Now
            });

        await _repository
            .GuardarCambiosAsync();
    }

    public async Task<List<NotificacionDto>>
        ObtenerMisNotificacionesAsync()
    {
        var lista =
            await _repository
                .ObtenerPorUsuarioAsync(
                    _usuarioActual.UsuarioId);

        return lista
            .OrderByDescending(
                x => x.Fecha)
            .Select(
                x =>
                    new NotificacionDto
                    {
                        Id = x.Id,
                        Titulo = x.Titulo,
                        Mensaje = x.Mensaje,
                        Leida = x.Leida,
                        Fecha = x.Fecha
                    })
            .ToList();
    }

    public async Task MarcarLeidaAsync(
        int id)
    {
        var notif =
            await _repository
                .ObtenerPorIdAsync(id);

        if (notif == null)
            return;

        notif.Leida = true;

        await _repository
            .GuardarCambiosAsync();
    }
}