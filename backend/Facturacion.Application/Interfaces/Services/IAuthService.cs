using Facturacion.Application.DTOs.Auth;

namespace Facturacion.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}