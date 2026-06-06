namespace Facturacion.Application.DTOs.Pagos;

public class RegistrarPagoDto
{
    public int FacturaId { get; set; }

    public decimal ValorPago { get; set; }
}