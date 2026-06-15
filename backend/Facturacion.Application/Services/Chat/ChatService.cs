using Facturacion.Application.Interfaces.Services;
using Facturacion.Application.DTOs.Chat;
using Facturacion.Application.Features.Chat;

namespace Facturacion.Application.Services.Chat;

public class ChatService : IChatService
{
    private readonly ICarteraService _carteraService;
    private readonly IFacturaService _facturaService;
    private readonly IPagoService _pagoService;
    private readonly IPendienteVentaService _pendienteService;
    private readonly IClienteService _clienteService;
    private readonly IUsuarioActualService _usuarioActual;

    public ChatService(
        ICarteraService carteraService,
        IFacturaService facturaService,
        IPagoService pagoService,
        IPendienteVentaService pendienteService,
        IClienteService clienteService,
        IUsuarioActualService usuarioActual)
    {
        _carteraService = carteraService;
        _facturaService = facturaService;
        _pagoService = pagoService;
        _pendienteService = pendienteService;
        _clienteService = clienteService;
        _usuarioActual = usuarioActual;
    }

    private async Task<ChatResponse>
    ConsultarCantidadClientes()
    {
        var clientes =
            await _clienteService
                .ObtenerTodosAsync();
        
        return new ChatResponse
        {
            Type = "text",
            Message =
                $"Actualmente existen {clientes.Count} clientes registrados."
        };
    }

    private async Task<ChatResponse>
        ConsultarFacturaMayorSaldo()
    {
        var facturas =
            await _facturaService
                .ObtenerTodasAsync();

        var factura =
            facturas
                .Where(
                    x => x.Saldo > 0)
                .OrderByDescending(
                    x => x.Saldo)
                .FirstOrDefault();

        if (factura == null)
        {
            return new ChatResponse
            {
                Type = "text",
                Message =
                    "No existen facturas con saldo pendiente."
            };
        }

        return new ChatResponse
        {
            Type = "text",
            Message =
                $"""
                La factura con mayor saldo pendiente es:

                Factura: #{factura.Codigo}
                Cliente: {factura.Cliente}
                Saldo: {factura.Saldo:N0}
                Fecha: {factura.FechaGen:dd/MM/yyyy}
                """
        };
    }

    private async Task<ChatResponse>
        ConsultarFacturadoMes()
    {
        var hoy =
            DateTime.Today;

        var facturas =
            await _facturaService
                .ObtenerTodasAsync();

        var mes =
            facturas.Where(
                x =>
                    x.FechaGen.Month ==
                    hoy.Month &&
                    x.FechaGen.Year ==
                    hoy.Year);

        var total =
            mes.Sum(
                x => x.ValorFactura);
        
        return new ChatResponse
        {
            Type = "text",
            Message =
                $"Este mes se han facturado {total:N0}."
        };
    }

    private async Task<ChatResponse>
    ConsultarRecaudadoMes()
    {

        // Verificar si tiene permisos para ver esta información
        if ((_usuarioActual.Rol ?? "")
            .Trim()
            .ToUpper() == "CONSULTA")
        {
            return new ChatResponse
            {
                Type = "text",
                Message =
                    "No tienes permisos para consultar el recaudo global del sistema."
            };
        }

        var hoy =
            DateTime.Today;

        var pagos =
            await _pagoService
                .ObtenerTodosAsync();

        var mes =
            pagos.Where(
                p =>
                    !p.Anulado &&
                    p.FechaPago.Month ==
                    hoy.Month &&
                    p.FechaPago.Year ==
                    hoy.Year);

        var total =
            mes.Sum(
                p => p.ValorPago);

        return new ChatResponse
        {
            Type = "text",
            Message =
                $"Este mes se han recaudado {total:N0}."
        };
        
    }

    private async Task<ChatResponse>
        ConsultarTopDeudores()
    {
        var cartera =
            await _carteraService
                .ObtenerPendientesAsync();

        var top =
            cartera
                .GroupBy(x => x.Cliente)
                .Select(g => new
                {
                    Cliente = g.Key,
                    Saldo = g.Sum(x => x.Saldo)
                })
                .OrderByDescending(x => x.Saldo)
                .Take(5)
                .ToList();

        return new ChatResponse
        {
            Type = "top_deudores",
            Message =
                """
                Top 5 clientes con mayor deuda.

                Para consultar el listado completo de clientes con saldo pendiente, dirígete al módulo Cartera.
                """,
            Data = top
        };
    }

    private ChatResponse Ayuda()
    {
        return new ChatResponse
        {
            Type = "text",
            Message =
                """
                Puedo ayudarte con:

                • ¿Cuánto debe Carlos?
                • ¿Cuánto se ha pagado hoy?
                • ¿Cuál es la cartera total?
                • ¿Qué cliente debe más?
                • ¿Cuáles son los 5 clientes que más deben?
                • ¿Cuál es la factura con mayor saldo?
                • ¿Cuánto se ha facturado este mes?
                • ¿Cuánto se ha recaudado este mes?
                • ¿Cuántos pendientes existen?
                """
        };
    }
    private ChatResponse Saludo()
    {
        return new ChatResponse
        {
            Type = "text",
            Message =
                """
                Hola 👋

                Puedo ayudarte con:

                • Cartera total
                • Pagos realizados hoy
                • Facturas
                • Pendientes
                • Cliente con mayor deuda
                • Top 5 deudores
                • Factura con mayor saldo
                • Consulta de clientes

                Escribe tu pregunta.
                """
        };
    }

    public async Task<ChatResponse> AskAsync(
        string message)
    {
        var texto = message
            .Trim()
            .ToLowerInvariant();

        var intent =
            IntentClassifier.Detect(texto);

        switch (intent)
        {
            case ChatIntent.ConsultarDeudaCliente:
                return await ConsultarDeudaCliente(texto);

            case ChatIntent.ConsultarPagosHoy:
                return await ConsultarPagosHoy();

            case ChatIntent.ConsultarFacturas:
                return await ConsultarFacturas();

            case ChatIntent.ConsultarPendientes:
                return await ConsultarPendientes();

            case ChatIntent.ConsultarCartera:
                return await ConsultarCarteraTotal();

            case ChatIntent.TopDeudores:
                return await ConsultarTopDeudores();

            case ChatIntent.ConsultarCliente:
                return await ConsultarCliente(texto);

            case ChatIntent.EstadoCliente:
                return await ConsultarEstadoCliente(texto);

            case ChatIntent.ConsultarFacturadoMes:
                return await ConsultarFacturadoMes();

            case ChatIntent.ConsultarRecaudadoMes:
                return await ConsultarRecaudadoMes();

            case ChatIntent.ConsultarMayorDeudor:
                return await ConsultarMayorDeudor();

            case ChatIntent.ConsultarFacturaMayorSaldo:
                return await ConsultarFacturaMayorSaldo();

            case ChatIntent.ConsultarSaludo:
                return Saludo();
            default:
                var clienteDetectado =
                    await DetectarCliente(texto);

                if (clienteDetectado != null)
                {
                    return clienteDetectado;
                }
                return Ayuda();
        }
    }

    private async Task<ChatResponse?>
        DetectarCliente(
            string texto)
    {
        var clientes =
            await _clienteService
                .ObtenerTodosAsync();

        texto =
            EntityExtractor
                .LimpiarPregunta(texto);

        var coincidencias =
            clientes
                .Where(
                    c =>
                        c.Nombre.Contains(
                            texto,
                            StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (!coincidencias.Any())
        {
            return null;
        }

        if (coincidencias.Count > 1)
        {
            return new ChatResponse
            {
                Type = "text",
                Message =
                    "Encontré varios clientes:\n\n" +
                    string.Join(
                        "\n",
                        coincidencias
                            .Take(5)
                            .Select(c => $"• {c.Nombre}")) +
                    "\n\nSé más específico."
            };
        }

        var cliente =
            coincidencias.First();

        if (cliente == null)
            return null;

        return await ConsultarCliente(
            $"cliente {cliente.Nombre}");
    }

    private async Task<ChatResponse>
        ConsultarEstadoCliente(
            string pregunta)
    {
        var clientes =
            await _clienteService
                .ObtenerTodosAsync();

        var cliente =
            clientes.FirstOrDefault(
                c =>
                    pregunta.Contains(
                        c.Nombre,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    pregunta.Contains(
                        c.Identificacion,
                        StringComparison.OrdinalIgnoreCase));

        if (cliente == null)
        {
            return new ChatResponse
            {
                Type = "text",
                Message =
                    "No encontré ese cliente."
            };
        }

        var cartera =
            await _carteraService
                .ObtenerPendientesAsync();

        var saldo =
            cartera
                .Where(
                    x => x.Cliente.Equals(
                        cliente.Nombre,
                        StringComparison.OrdinalIgnoreCase))
                .Sum(
                    x => x.Saldo);

        if (saldo <= 0)
        {
            return new ChatResponse
            {
                Type = "text",
                Message =
                    $"{cliente.Nombre} se encuentra al día."
            };
        }
        
        return new ChatResponse
        {
            Type = "text",
            Message =
                $"{cliente.Nombre} tiene una deuda pendiente de {saldo:N0}."
        };
    }

    private async Task<ChatResponse>
        ConsultarDeudaCliente(
            string pregunta)
    {
        var nombre =
            ExtraerNombreCliente(
                pregunta);

        var clientes =
            await _clienteService
                .ObtenerTodosAsync();

        var coincidencias =
            clientes
                .Where(
                    c =>
                        c.Nombre.Contains(
                            nombre,
                            StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (!coincidencias.Any())
        {
            return new ChatResponse
            {
                Type = "text",
                Message =
                    $"No encontré ningún cliente llamado '{nombre}'."
            };
        }

        // Si hay coincidencia exacta usarla
        var cliente =
            coincidencias.FirstOrDefault(
                c =>
                    c.Nombre.Equals(
                        nombre,
                        StringComparison.OrdinalIgnoreCase));

        // Si no existe coincidencia exacta y hay varios resultados
        if (cliente == null &&
            coincidencias.Count > 1)
        {
            return new ChatResponse
            {
                Type = "text",
                Message =
                    "Encontré varios clientes:\n\n" +
                    string.Join(
                        "\n",
                        coincidencias
                            .Take(10)
                            .Select(
                                c => $"• {c.Nombre}")
                    ) +
                    "\n\nEscribe el nombre completo del cliente."
            };
        }

        cliente ??=
            coincidencias.First();

        var cartera =
            await _carteraService
                .ObtenerPendientesAsync();

        var saldo =
            cartera
                .Where(
                    x =>
                        x.Cliente.Equals(
                            cliente.Nombre,
                            StringComparison.OrdinalIgnoreCase))
                .Sum(
                    x => x.Saldo);

        if (saldo <= 0)
        {
            return new ChatResponse
            {
                Type = "text",
                Message =
                    $"{cliente.Nombre} se encuentra al día y no tiene saldo pendiente."
            };
        }

        return new ChatResponse
        {
            Type = "text",
            Message =
                $"{cliente.Nombre} tiene un saldo pendiente de {saldo:N0}."
        };
    }

    private async Task<ChatResponse>
        ConsultarPagosHoy()
    {
        // Verificar si tiene permiso para ver esta información
        if ((_usuarioActual.Rol ?? "")
            .Trim()
            .ToUpper() == "CONSULTA")
        {
            return new ChatResponse
            {
                Type = "text",
                Message =
                    "No tienes permisos para consultar pagos globales del sistema."
            };
        }

        var pagos =
            await _pagoService
                .ObtenerTodosAsync();

        var hoy =
            DateTime.Today;

        var pagosHoy =
            pagos
                .Where(
                    p => !p.Anulado &&
                    p.FechaPago.Date == hoy)
                .ToList();

        var total =
            pagosHoy.Sum(
                p => p.ValorPago);

        return new ChatResponse
        {
            Type = "text",
            Message =
                $"Hoy se registraron {pagosHoy.Count} pagos por {total:N0}."
        };
    }

    private async Task<ChatResponse>
        ConsultarFacturas()
    {
        var facturas =
            await _facturaService
                .ObtenerTodasAsync();

        var total =
            facturas.Sum(
                f => f.ValorFactura);

        return new ChatResponse
        {
            Type = "text",
            Message =
                $"Existen {facturas.Count} facturas por un valor acumulado de {total:N0}."
        };
    }

    private async Task<ChatResponse>
        ConsultarPendientes()
    {
        var pendientes =
            await _pendienteService
                .ObtenerTodosAsync();

        var total =
            pendientes.Sum(
                p => p.Total);

        return new ChatResponse
        {
            Type = "text",
            Message =
                $"Existen {pendientes.Count} pendientes por un valor de {total:N0}."
        };
    }

    private async Task<ChatResponse>
        ConsultarCarteraTotal()
    {
        var cartera =
            await _carteraService
                .ObtenerPendientesAsync();

        var saldo =
            cartera.Sum(
                x => x.Saldo);

        return new ChatResponse
        {
            Type = "text",
            Message =
                $"La cartera pendiente total es de {saldo:N0}."
        };
    }

    private async Task<ChatResponse>
        ConsultarMayorDeudor()
    {
        var cartera =
            await _carteraService
                .ObtenerPendientesAsync();

        var cliente =
            cartera
                .GroupBy(
                    x => x.Cliente)
                .Select(
                    g => new
                    {
                        Cliente = g.Key,
                        Saldo = g.Sum(
                            x => x.Saldo)
                    })
                .OrderByDescending(
                    x => x.Saldo)
                .FirstOrDefault();

        if (cliente == null)
        {                
            return new ChatResponse
            {
                Type = "text",
                Message =
                    "No existen registros de cartera."
            };
        }
                
        return new ChatResponse
        {
            Type = "text",
            Message =
                $"El cliente con mayor deuda es {cliente.Cliente} con un saldo pendiente de {cliente.Saldo:N0}."
        };
    }

    private string ExtraerNombreCliente(
        string pregunta)
    {
        return pregunta
            .Replace("¿", "")
            .Replace("?", "")
            .Replace("cuánto debe", "", StringComparison.OrdinalIgnoreCase)
            .Replace("cuanto debe", "", StringComparison.OrdinalIgnoreCase)
            .Trim();
    }

    private async Task<ChatResponse>
        ConsultarCliente(string pregunta)
    {
        var clientes =
            await _clienteService
                .ObtenerTodosAsync();

        var cartera =
            await _carteraService
                .ObtenerPendientesAsync();

        var texto =
            pregunta
                .Replace("cliente", "")
                .Replace("buscar", "")
                .Replace("estado", "")
                .Trim();

        var cliente =
            clientes.FirstOrDefault(
                c => c.Nombre.Contains(
                    texto,
                    StringComparison
                        .OrdinalIgnoreCase));

        if (cliente == null)
                
            return new ChatResponse
            {
                Type = "text",
                Message =
                    "No encontré ese cliente."
            };

        var deuda =
            cartera
                .Where(
                    x => x.Cliente.Equals(
                        cliente.Nombre,
                        StringComparison
                            .OrdinalIgnoreCase))
                .ToList();

        var saldo =
            deuda.Sum(
                x => x.Saldo);
             
        return new ChatResponse
        {
            Type = "text",
            Message =
                $"""
            Cliente: {cliente.Nombre}

            Saldo pendiente:
            {saldo:N0}

            Facturas pendientes:
            {deuda.Count}
            """
        };
    }

    private string ExtraerBusquedaCliente(
        string pregunta)
    {
        return pregunta
            .Replace("cliente", "",
                StringComparison.OrdinalIgnoreCase)
            .Replace("buscar", "",
                StringComparison.OrdinalIgnoreCase)
            .Replace("estado", "",
                StringComparison.OrdinalIgnoreCase)
            .Trim();
    }
    
}