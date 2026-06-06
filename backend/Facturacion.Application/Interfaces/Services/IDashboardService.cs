using Facturacion.Application.DTOs.Dashboard;

namespace Facturacion.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardDto> ObtenerResumenAsync();
}