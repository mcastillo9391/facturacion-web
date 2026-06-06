namespace Facturacion.Application.DTOs.Usuarios;

public class ActualizarUsuarioDto
{
    public int RolId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public bool Activo { get; set; }
}