using Facturacion.Application.DTOs.Auth;
using Facturacion.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
            return Unauthorized();

        return Ok(result);
    }
}