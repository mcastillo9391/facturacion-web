using Facturacion.Application.DTOs.Usuarios;

namespace Facturacion.Application.Interfaces.Services;

public interface IUsuarioService
{
    Task<List<UsuarioDto>> ObtenerTodosAsync();

    Task<UsuarioDto?> ObtenerPorIdAsync(int id);

    Task<int> CrearAsync(CrearUsuarioDto dto);

    Task<bool> ActualizarAsync(
        int id,
        ActualizarUsuarioDto dto);

    Task<bool> DesactivarAsync(int id);
}