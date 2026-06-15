namespace Facturacion.Application.Features.Chat;

public enum ChatIntent
{
    Unknown,

    ConsultarSaludo,

    ConsultarCartera,
    ConsultarPagosHoy,
    ConsultarFacturas,
    ConsultarPendientes,
    ConsultarDeudaCliente,
    ConsultarCliente,
    EstadoCliente,
    TopDeudores,
    ConsultarFacturadoMes,
    ConsultarRecaudadoMes,
    ConsultarMayorDeudor,
    ConsultarFacturaMayorSaldo
}