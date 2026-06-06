namespace Facturacion.Application.DTOs.Clientes;

public class ClienteDto
{
    public int IdCli { get; set; }

    public string Identificacion { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public bool Activo { get; set; }
}