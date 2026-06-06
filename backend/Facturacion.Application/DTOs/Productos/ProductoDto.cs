namespace Facturacion.Application.DTOs.Productos;

public class ProductoDto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public decimal ValorCosto { get; set; }

    public decimal ValorVenta { get; set; }

    public bool Activo { get; set; }
}