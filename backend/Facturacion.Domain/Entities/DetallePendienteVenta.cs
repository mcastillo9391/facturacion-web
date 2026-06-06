namespace Facturacion.Domain.Entities;

public class DetallePendienteVenta
{
    public int DetallePendienteVentaId { get; set; }

    public int PendienteVentaId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal ValorUnitario { get; set; }

    public decimal Descuento { get; set; }

    public decimal Total { get; set; }

    public PendienteVenta PendienteVenta { get; set; } = null!;

    public Producto Producto { get; set; } = null!;
}