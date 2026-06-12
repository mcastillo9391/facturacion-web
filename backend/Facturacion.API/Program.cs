using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Facturacion.Infrastructure.Data;
using Facturacion.Infrastructure.Repositories;
using Facturacion.Application.Interfaces.Repositories;
using Facturacion.Application.Interfaces.Services;
using Facturacion.Application.Services.Usuarios;
using Facturacion.Application.Services.Productos;
using Facturacion.Application.Services.Clientes;
using Facturacion.Application.Services.Ventas;
using Facturacion.Application.Services.Recaudo;
using Facturacion.Application.Services.Gastos;
using Facturacion.Application.Services.Auth;
using Facturacion.Application.Services.Dashboard;

var builder = WebApplication.CreateBuilder(args);

// CORS
var allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins ?? new string[] {})
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
});

// Dependency Injection
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Usuarios
builder.Services.AddScoped<IUsuarioService,
    UsuarioService>();

// Productos
builder.Services.AddScoped<
    IProductoRepository,
    ProductoRepository>();

builder.Services.AddScoped<
    IProductoService,
    ProductoService>();

// Clientes
builder.Services.AddScoped<
    IClienteRepository,
    ClienteRepository>();

builder.Services.AddScoped<
    IClienteService,
    ClienteService>();

// PendientesVenta
builder.Services.AddScoped<
    IPendienteVentaRepository,
    PendienteVentaRepository>();

builder.Services.AddScoped<
    IDetallePendienteVentaRepository,
    DetallePendienteVentaRepository>();

// Ventas
builder.Services.AddScoped<
    IPendienteVentaService,
    PendienteVentaService>();

// Facturas
builder.Services.AddScoped<
    IFacturaRepository,
    FacturaRepository>();

builder.Services.AddScoped<
    IFacturaService,
    FacturaService>();

// Pagos
builder.Services.AddScoped<
    IPagoRepository,
    PagoRepository>();

builder.Services.AddScoped<
    IPagoService,
    PagoService>();

// Envios
builder.Services.AddScoped<IEnvioRepository,
    EnvioRepository>();

builder.Services.AddScoped<IEnvioService,
    EnvioService>();

// Dashboard
builder.Services
    .AddScoped<
        IDashboardService,
        DashboardService>();

builder.Services.AddScoped<
    ICarteraService,
    CarteraService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<
    IUsuarioActualService,
    UsuarioActualService>();

// JWT
var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey!))
            };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Swagger
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
//app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();