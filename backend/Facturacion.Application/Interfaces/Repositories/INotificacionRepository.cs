using Facturacion.Domain.Entities;

public interface INotificacionRepository
{
    Task CrearAsync(
        Notificacion notificacion);

    Task<List<Notificacion>>
        ObtenerPorUsuarioAsync(
            int usuarioId);

    Task<Notificacion?>
        ObtenerPorIdAsync(
            int id);

    Task GuardarCambiosAsync();
}