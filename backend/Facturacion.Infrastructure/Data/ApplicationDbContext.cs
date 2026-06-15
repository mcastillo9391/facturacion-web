using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Rol> Roles => Set<Rol>();

    public DbSet<Producto> Productos => Set<Producto>();

    public DbSet<Cliente> Clientes => Set<Cliente>();

    public DbSet<Notificacion> Notificaciones { get; set; }

    public DbSet<PendienteVenta> PendientesVenta
        => Set<PendienteVenta>();

    public DbSet<DetallePendienteVenta>
        DetallesPendienteVenta
        => Set<DetallePendienteVenta>();

    public DbSet<Factura> Facturas => Set<Factura>();

    public DbSet<DetalleFactura> DetalleFacturas => Set<DetalleFactura>();

    public DbSet<Pago> Pagos => Set<Pago>();

    public DbSet<DetallePago> DetallesPago
        => Set<DetallePago>();

    public DbSet<Envio> Envios => Set<Envio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cargar todas las configuraciones del assembly (Factura, Cliente, etc.)
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        // Mapeo explícito de UsuarioAsig en PendienteVenta
        // Se hace aquí directamente para garantizar que EF lo incluya en el modelo
        modelBuilder.Entity<PendienteVenta>(entity =>
        {
            entity.Property(x => x.UsuarioAsig)
                .HasColumnName("UsuarioAsig");

            entity.HasOne(x => x.UsuarioAsigNavigation)
                .WithMany()
                .HasForeignKey(x => x.UsuarioAsig)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });

        base.OnModelCreating(modelBuilder);
    }
}
