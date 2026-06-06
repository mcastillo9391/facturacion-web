using BCrypt.Net;
using Facturacion.Application.DTOs.Usuarios;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;
using Facturacion.Domain.Entities;

namespace Facturacion.Application.Services.Usuarios;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(
        IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<List<UsuarioDto>> ObtenerTodosAsync()
    {
        var usuarios =
            await _usuarioRepository.ObtenerTodosAsync();

        return usuarios.Select(x => new UsuarioDto
        {
            UsuarioId = x.UsuarioId,
            RolId = x.RolId,
            Nombre = x.Nombre,
            Username = x.Username,
            Rol = x.Rol.Nombre,
            Activo = x.Activo
        }).ToList();
    }

    public async Task<UsuarioDto?> ObtenerPorIdAsync(int id)
    {
        var usuario =
            await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario == null)
            return null;

        return new UsuarioDto
        {
            UsuarioId = usuario.UsuarioId,
            RolId = usuario.RolId,
            Nombre = usuario.Nombre,
            Username = usuario.Username,
            Rol = usuario.Rol.Nombre,
            Activo = usuario.Activo
        };
    }

    public async Task<int> CrearAsync(
        CrearUsuarioDto dto)
    {
        var usuario = new Usuario
        {
            RolId = dto.RolId,
            Nombre = dto.Nombre,
            Username = dto.Username,
            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    dto.Password),
            Activo = dto.Activo
        };

        await _usuarioRepository
            .AgregarAsync(usuario);

        await _usuarioRepository
            .GuardarCambiosAsync();

        return usuario.UsuarioId;
    }

    public async Task<bool> ActualizarAsync(
        int id,
        ActualizarUsuarioDto dto)
    {
        var usuario =
            await _usuarioRepository
                .ObtenerPorIdAsync(id);

        if (usuario == null)
            return false;

        usuario.RolId = dto.RolId;
        usuario.Nombre = dto.Nombre;
        usuario.Username = dto.Username;
        usuario.Activo = dto.Activo;

        await _usuarioRepository
            .ActualizarAsync(usuario);

        await _usuarioRepository
            .GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> DesactivarAsync(int id)
    {
        var usuario =
            await _usuarioRepository
                .ObtenerPorIdAsync(id);

        if (usuario == null)
            return false;

        usuario.Activo = false;

        await _usuarioRepository
            .ActualizarAsync(usuario);

        await _usuarioRepository
            .GuardarCambiosAsync();

        return true;
    }
}