namespace Facturacion.Application.DTOs.Pagos;

public class PagoDto
{
    public int IdPago { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public DateTime FechaPago { get; set; }

    public decimal ValorPago { get; set; }
}