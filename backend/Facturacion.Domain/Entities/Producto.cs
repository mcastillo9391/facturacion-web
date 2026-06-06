namespace Facturacion.Domain.Entities;

public class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public decimal ValorCosto { get; set; }

    public decimal ValorVenta { get; set; }

    public bool Activo { get; set; }

    public ICollection<DetallePendienteVenta>
    DetallesPendienteVenta
    = new List<DetallePendienteVenta>();
}