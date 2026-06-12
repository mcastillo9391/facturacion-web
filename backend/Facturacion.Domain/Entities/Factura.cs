namespace Facturacion.Domain.Entities;

public class Factura
{
    public int Codigo { get; set; }

    public int IdCli { get; set; }

    public DateTime FechaGen { get; set; }

    public decimal ValorFactura { get; set; }

    public decimal? ValorAbonado { get; set; }

    public int? UsuarioId { get; set; }

    public string? Estado { get; set; }

    public Cliente Cliente { get; set; } = null!;

    public Usuario? Usuario { get; set; }

    public virtual Usuario? UsuarioAsigNavigation { get; set; } 

    public int? UsuarioAsig { get; set; }

    public ICollection<DetalleFactura> Detalles
    {
        get;
        set;
    } = new List<DetalleFactura>();

    public ICollection<DetallePago>
    DetallesPago
    = new List<DetallePago>();
}