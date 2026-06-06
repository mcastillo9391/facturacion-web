namespace Facturacion.Application.DTOs.PendientesVenta;

public class PendienteVentaDetalleDto
{
    public int PendienteVentaId { get; set; }

    public int ClienteId { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public List<DetallePendienteVentaDto>
        Detalles { get; set; }
        = new();
}