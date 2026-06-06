namespace Facturacion.Domain.Entities;

public class DetalleFactura
{
    public int Codigo { get; set; }

    public int Producto { get; set; }

    public int Factura { get; set; }

    public float CostoUnitario { get; set; }

    public float ValorUnitario { get; set; }

    public float? Descuento { get; set; }

    public int Cantidad { get; set; }

    public float Total { get; set; }

    public Producto ProductoNavigation { get; set; } = null!;

    public Factura FacturaNavigation { get; set; } = null!;
}