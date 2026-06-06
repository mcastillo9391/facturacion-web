using Facturacion.Application.DTOs.Clientes;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;
using Facturacion.Domain.Entities;

namespace Facturacion.Application.Services.Clientes;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(
        IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<List<ClienteDto>> ObtenerTodosAsync()
    {
        var clientes =
            await _clienteRepository.ObtenerTodosAsync();

        return clientes
            .Select(x => new ClienteDto
            {
                IdCli = x.IdCli,
                Identificacion = x.Identificacion,
                Nombre = x.Nombre,
                Direccion = x.Direccion,
                Telefono = x.Telefono,
                Activo = x.Activo
            })
            .ToList();
    }

    public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
    {
        var cliente =
            await _clienteRepository.ObtenerPorIdAsync(id);

        if (cliente == null)
            return null;

        return new ClienteDto
        {
            IdCli = cliente.IdCli,
            Identificacion = cliente.Identificacion,
            Nombre = cliente.Nombre,
            Direccion = cliente.Direccion,
            Telefono = cliente.Telefono,
            Activo = cliente.Activo
        };
    }

    public async Task<int> CrearAsync(
        CrearClienteDto dto)
    {
        var existente =
            await _clienteRepository
                .ObtenerPorIdentificacionAsync(
                    dto.Identificacion);

        if (existente != null)
            throw new Exception(
                "Ya existe un cliente con esa identificación.");

        var cliente = new Cliente
        {
            Identificacion = dto.Identificacion,
            Nombre = dto.Nombre,
            Direccion = dto.Direccion,
            Telefono = dto.Telefono,
            FechaCreacion = DateTime.Now,
            Activo = true
        };

        await _clienteRepository
            .AgregarAsync(cliente);

        await _clienteRepository
            .GuardarCambiosAsync();

        return cliente.IdCli;
    }

    public async Task<bool> ActualizarAsync(
        int id,
        ActualizarClienteDto dto)
    {
        var cliente =
            await _clienteRepository
                .ObtenerPorIdAsync(id);

        if (cliente == null)
            return false;

        cliente.Identificacion = dto.Identificacion;
        cliente.Nombre = dto.Nombre;
        cliente.Direccion = dto.Direccion;
        cliente.Telefono = dto.Telefono;
        cliente.Activo = dto.Activo;

        await _clienteRepository
            .ActualizarAsync(cliente);

        await _clienteRepository
            .GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> DesactivarAsync(
        int id)
    {
        var cliente =
            await _clienteRepository
                .ObtenerPorIdAsync(id);

        if (cliente == null)
            return false;

        cliente.Activo = false;

        await _clienteRepository
            .ActualizarAsync(cliente);

        await _clienteRepository
            .GuardarCambiosAsync();

        return true;
    }
}