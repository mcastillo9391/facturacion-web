namespace Facturacion.Application.Features.Chat;

public static class IntentClassifier
{
    public static ChatIntent Detect(string text)
    {
        text = text
            .Trim()
            .ToLowerInvariant();

        if (
            text == "hola" ||
            text == "buenas" ||
            text == "buenos dias" ||
            text == "buenos días" ||
            text == "buenas tardes" ||
            text == "buenas noches"
        )
        {
            return ChatIntent.ConsultarSaludo;
        }

        // Cliente con mayor deuda
        if (
            text.Contains("cliente debe más") ||
            text.Contains("cliente debe mas") ||
            text.Contains("quien debe más") ||
            text.Contains("quien debe mas") ||
            text.Contains("mayor deudor")
        )
        {
            return ChatIntent.ConsultarMayorDeudor;
        }
        
        // Deuda de cliente
        if (
            text.Contains("cuanto debe ") ||
            text.Contains("cuánto debe ")
        )
        {
            return ChatIntent.ConsultarDeudaCliente;
        }

        // Factura con mayor saldo
        if (
            text.Contains("factura con mayor saldo") ||
            text.Contains("mayor saldo") ||
            text.Contains("factura más alta") ||
            text.Contains("factura mas alta")
        )
        {
            return ChatIntent.ConsultarFacturaMayorSaldo;
        }
        // Top deudores
        if (
            text.Contains("más deben") ||
            text.Contains("mas deben") ||
            text.Contains("top deudores") ||
            text.Contains("mayor deuda") ||
            text.Contains("cliente debe más") ||
            text.Contains("cliente debe mas") ||

            text.Contains("deudores") ||
            text.Contains("morosos") ||
            text.Contains("clientes morosos") ||
            text.Contains("clientes con deuda") ||
            text.Contains("clientes con saldo") ||
            text.Contains("saldo pendiente") ||
            text.Contains("quienes deben") ||
            text.Contains("quiénes deben") ||
            text.Contains("quienes tienen deuda") ||
            text.Contains("quiénes tienen deuda") ||
            text.Contains("quienes tienen saldo pendiente") ||
            text.Contains("quiénes tienen saldo pendiente")
        )
        {
            return ChatIntent.TopDeudores;
        }

        // Cartera
        if (
            text.Contains("cartera") ||
            text.Contains("por cobrar") ||
            text.Contains("deuda total") ||
            text.Contains("saldo total")
        )
        {
            return ChatIntent.ConsultarCartera;
        }

        // Estado de cliente
        if (
            text.Contains("tiene deuda") ||
            text.Contains("está al día") ||
            text.Contains("esta al dia")
        )
        {
            return ChatIntent.EstadoCliente;
        }

        // Consulta explícita de cliente
        if (
            text.StartsWith("cliente ") ||
            text.StartsWith("buscar cliente ") ||
            text.StartsWith("estado cliente ")
        )
        {
            return ChatIntent.ConsultarCliente;
        }

        // Pagos del día
        if (
            text.Contains("pagado hoy") ||
            text.Contains("pagos hoy") ||
            text.Contains("recaudado hoy")
        )
        {
            return ChatIntent.ConsultarPagosHoy;
        }

        // Facturación mensual
        if (
            text.Contains("facturado este mes") ||
            text.Contains("ventas del mes")
        )
        {
            return ChatIntent.ConsultarFacturadoMes;
        }

        // Recaudo mensual
        if (
            text.Contains("recaudado este mes") ||
            text.Contains("cobrado este mes")
        )
        {
            return ChatIntent.ConsultarRecaudadoMes;
        }

        // Facturas
        if (
            text.Contains("factura") &&
            text.Contains("mayor saldo")
        )
        {
            return ChatIntent.ConsultarFacturas;
        }

        // Pendientes
        if (
            text.Contains("pendientes")
        )
        {
            return ChatIntent.ConsultarPendientes;
        }

        return ChatIntent.Unknown;
    }
}