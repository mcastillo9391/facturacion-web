namespace Facturacion.Application.DTOs.Productos;

public class CrearProductoDto
{
    public string Nombre { get; set; } = string.Empty;

    public decimal ValorCosto { get; set; }

    public decimal ValorVenta { get; set; }
}