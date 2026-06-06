namespace Facturacion.Application.DTOs.Productos;

public class ActualizarProductoDto
{
    public string Nombre { get; set; } = string.Empty;

    public decimal ValorCosto { get; set; }

    public decimal ValorVenta { get; set; }

    public bool Activo { get; set; }
}