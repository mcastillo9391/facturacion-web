namespace Facturacion.Application.DTOs.Envios;

public class EnvioResponseDto
{
    public int Codigo { get; set; }

    public DateTime FechaEnvio { get; set; }

    public decimal ValorEnvio { get; set; }

    public string? Ciudad { get; set; }
}