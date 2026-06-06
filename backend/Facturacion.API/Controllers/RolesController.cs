using Facturacion.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class RolesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RolesController(
        ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerRoles()
    {
        var roles =
            await _context.Roles
                .Select(r => new
                {
                    rolId = r.RolId,
                    nombre = r.Nombre
                })
                .ToListAsync();

        return Ok(roles);
    }
}