namespace Facturacion.Domain.Entities;

public class Notificacion
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Mensaje { get; set; } = string.Empty;

    public bool Leida { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}