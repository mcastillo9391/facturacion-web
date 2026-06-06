namespace Facturacion.Domain.Entities;

public class DetallePago
{
    public int Codigo { get; set; }

    public int IdPago { get; set; }

    public int Factura { get; set; }

    public decimal ValorPago { get; set; }

    public Pago Pago { get; set; } = null!;

    public Factura FacturaNavigation { get; set; } = null!;
}