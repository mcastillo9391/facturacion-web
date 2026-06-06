namespace Facturacion.Domain.Entities;

public class PendienteVenta
{
    public int PendienteVentaId { get; set; }

    public int ClienteId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string Estado { get; set; } = string.Empty;

    public string? Observaciones { get; set; }

    public Cliente Cliente { get; set; } = null!;

    public Usuario Usuario { get; set; } = null!;

    public ICollection<DetallePendienteVenta> Detalles
    {
        get;
        set;
    } = new List<DetallePendienteVenta>();
}