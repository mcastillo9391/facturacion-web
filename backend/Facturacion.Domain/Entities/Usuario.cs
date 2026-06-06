namespace Facturacion.Domain.Entities;

public class Usuario
{
    public int UsuarioId { get; set; }

    public int RolId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public Rol Rol { get; set; } = null!;

    public ICollection<PendienteVenta>
    PendientesVenta
    = new List<PendienteVenta>();
}