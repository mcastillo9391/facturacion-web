using Facturacion.Domain.Entities;

namespace Facturacion.Application.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorUsernameAsync(string username);

    Task<List<Usuario>> ObtenerTodosAsync();

    Task<Usuario?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Usuario usuario);
    
    Task ActualizarAsync(Usuario usuario);

    Task GuardarCambiosAsync();
}