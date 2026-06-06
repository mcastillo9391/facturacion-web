namespace Facturacion.Application.DTOs.Facturas;

public class DetalleFacturaDto
{
    public int Codigo { get; set; }

    public string Producto { get; set; } = string.Empty;

    public int Cantidad { get; set; }

    public decimal ValorUnitario { get; set; }

    public decimal Descuento { get; set; }

    public decimal Total { get; set; }
}