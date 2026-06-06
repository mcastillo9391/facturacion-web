using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Facturacion.Application.DTOs.Auth;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Facturacion.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto request)
    {
        var usuario =
            await _usuarioRepository
                .ObtenerPorUsernameAsync(request.Username);

        if (usuario == null)
            return null;

        bool passwordValido =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                usuario.PasswordHash);

        if (!passwordValido)
            return null;

        var token = GenerarToken(usuario);

        return new LoginResponseDto
        {
            Token = token,
            Username = usuario.Username,
            Nombre = usuario.Nombre,
            Rol = usuario.Rol.Nombre
        };
    }

    private string GenerarToken(
        Facturacion.Domain.Entities.Usuario usuario)
    {
        var jwtKey =
            _configuration["Jwt:Key"];

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                usuario.UsuarioId.ToString()),

            new Claim(
                ClaimTypes.Name,
                usuario.Username),

            new Claim(
                ClaimTypes.Role,
                usuario.Rol.Nombre)
        };

        var token =
            new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(
                        _configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}