namespace Facturacion.Application.DTOs.Clientes;

public class CrearClienteDto
{
    public string Identificacion { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;
}