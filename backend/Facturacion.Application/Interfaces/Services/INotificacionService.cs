using Facturacion.Application.DTOs.Notificaciones;

public interface INotificacionService
{
    Task CrearAsync(
        int usuarioId,
        string titulo,
        string mensaje);

    Task<List<NotificacionDto>>
        ObtenerMisNotificacionesAsync();

    Task MarcarLeidaAsync(
        int id);
}