namespace Facturacion.Application.DTOs.Facturas;

public class FacturaDto
{
    public int Codigo { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public DateTime FechaGen { get; set; }

    public decimal ValorFactura { get; set; }

    public decimal ValorAbonado { get; set; }

    public decimal Saldo =>
        ValorFactura - ValorAbonado;

    public string Estado { get; set; } = string.Empty;
}