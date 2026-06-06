using Facturacion.Application.DTOs.Envios;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;
using Facturacion.Domain.Entities;

namespace Facturacion.Application.Services.Gastos;

public class EnvioService : IEnvioService
{
    private readonly IEnvioRepository _envioRepository;

    public EnvioService(
        IEnvioRepository envioRepository)
    {
        _envioRepository = envioRepository;
    }

    public async Task<List<EnvioResponseDto>>
        ObtenerTodosAsync()
    {
        var envios =
            await _envioRepository.ObtenerTodosAsync();

        return envios
            .Select(x => new EnvioResponseDto
            {
                Codigo = x.Codigo,
                FechaEnvio = x.FechaEnvio,
                ValorEnvio = x.ValorEnvio,
                Ciudad = x.Ciudad
            })
            .ToList();
    }

    public async Task<EnvioResponseDto?>
        ObtenerPorIdAsync(int id)
    {
        var envio =
            await _envioRepository.ObtenerPorIdAsync(id);

        if (envio == null)
            return null;

        return new EnvioResponseDto
        {
            Codigo = envio.Codigo,
            FechaEnvio = envio.FechaEnvio,
            ValorEnvio = envio.ValorEnvio,
            Ciudad = envio.Ciudad
        };
    }

    public async Task CrearAsync(
        CrearEnvioDto dto)
    {
        var envio = new Envio
        {
            FechaEnvio = DateTime.Now,
            ValorEnvio = dto.ValorEnvio,
            Ciudad = dto.Ciudad
        };

        await _envioRepository.AgregarAsync(envio);
        await _envioRepository.GuardarCambiosAsync();
    }

    public async Task ActualizarAsync(
        int id,
        ActualizarEnvioDto dto)
    {
        var envio =
            await _envioRepository.ObtenerPorIdAsync(id);

        if (envio == null)
            throw new Exception("Envío no encontrado");

        envio.ValorEnvio = dto.ValorEnvio;
        envio.Ciudad = dto.Ciudad;

        await _envioRepository.ActualizarAsync(envio);
        await _envioRepository.GuardarCambiosAsync();
    }
}