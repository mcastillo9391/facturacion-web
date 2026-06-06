namespace Facturacion.Domain.Entities;

public class Envio
{
    public int Codigo { get; set; }

    public DateTime FechaEnvio { get; set; }

    public decimal ValorEnvio { get; set; }

    public string? Ciudad { get; set; }
}