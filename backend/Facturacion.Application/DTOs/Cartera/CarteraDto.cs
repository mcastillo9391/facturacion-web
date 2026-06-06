namespace Facturacion.Application.DTOs.Cartera;

public class CarteraDto
{
    public int FacturaId { get; set; }

    public string Cliente { get; set; }
        = string.Empty;

    public DateTime FechaFactura { get; set; }

    public decimal ValorFactura { get; set; }

    public decimal ValorAbonado { get; set; }

    public decimal Saldo { get; set; }

    public string Estado { get; set; }
        = string.Empty;
}