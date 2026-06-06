namespace Facturacion.Application.DTOs.Dashboard;

public class DashboardDto
{
    public decimal VentasHoy { get; set; }

    public decimal VentasMes { get; set; }

    public decimal RecaudosHoy { get; set; }

    public decimal RecaudosMes { get; set; }

    public decimal GastosEnvioMes { get; set; }

    public decimal CarteraPendiente { get; set; }

    public int FacturasPendientes { get; set; }

    public int ClientesActivos { get; set; }

    public int ProductosActivos { get; set; }
}