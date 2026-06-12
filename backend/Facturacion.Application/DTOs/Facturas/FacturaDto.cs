public class FacturaDto
{
    public int Codigo { get; set; }

    public int ClienteId { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public DateTime FechaGen { get; set; }

    public int UsuarioId { get; set; }

    public int? UsuarioAsig { get; set; }

    public string UsuarioCreador { get; set; }

    public string UsuarioAsignado { get; set; }

    public decimal ValorFactura { get; set; }

    public decimal ValorAbonado { get; set; }

    public decimal Saldo { get; set; }

    public string Estado { get; set; }
}