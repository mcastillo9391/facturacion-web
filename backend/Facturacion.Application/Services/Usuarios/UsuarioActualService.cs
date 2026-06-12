using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class UsuarioActualService : IUsuarioActualService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioActualService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UsuarioId =>
        int.Parse(
            _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? "0");

    public string Rol =>
        _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.Role)?.Value
        ?? "";
}