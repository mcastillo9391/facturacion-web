namespace Facturacion.Application.DTOs.Usuarios;

public class UsuarioDto
{
    public int UsuarioId { get; set; }

    public int RolId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;

    public bool Activo { get; set; }
}