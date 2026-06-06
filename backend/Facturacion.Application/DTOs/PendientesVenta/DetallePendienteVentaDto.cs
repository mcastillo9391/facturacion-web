namespace Facturacion.Application.DTOs.PendientesVenta;

public class DetallePendienteVentaDto
{
    public int DetallePendienteVentaId { get; set; }

    public int ProductoId { get; set; }

    public string Producto { get; set; } = string.Empty;

    public int Cantidad { get; set; }

    public decimal ValorUnitario { get; set; }

    public decimal Descuento { get; set; }

    public decimal Total { get; set; }
}