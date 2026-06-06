namespace Facturacion.Application.DTOs.Pagos;

public class PagoResponseDto
{
    public int IdPago { get; set; }

    public int IdCli { get; set; }

    public string Cliente { get; set; }
        = string.Empty;

    public DateTime FechaPago { get; set; }

    public DateTime? FechaAnulacion { get; set; }

    public decimal ValorPago { get; set; }

    public bool Anulado =>
        FechaAnulacion != null;
}