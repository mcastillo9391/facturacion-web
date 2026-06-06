namespace Facturacion.Application.DTOs.Usuarios;

public class CrearUsuarioDto
{
    public int RolId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}