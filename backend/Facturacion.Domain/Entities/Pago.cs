namespace Facturacion.Domain.Entities;

public class Pago
{
    public int IdPago { get; set; }

    public int IdCli { get; set; }

    public DateTime FechaPago { get; set; }

    public DateTime? FechaAnulacion { get; set; }

    public decimal ValorPago { get; set; }

    public Cliente Cliente { get; set; } = null!;

    public ICollection<DetallePago>
        DetallesPago
        = new List<DetallePago>();
}