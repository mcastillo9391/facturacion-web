namespace Facturacion.Application.DTOs.PendientesVenta;

public class AgregarProductoPendienteDto
{
    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal Descuento { get; set; }
}