using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configurations;

public class DetallePendienteVentaConfiguration
    : IEntityTypeConfiguration<DetallePendienteVenta>
{
    public void Configure(
        EntityTypeBuilder<DetallePendienteVenta> builder)
    {
        builder.ToTable("DetallesPendienteVenta");

        builder.HasKey(x => x.DetallePendienteVentaId);

        builder.Property(x => x.DetallePendienteVentaId)
            .HasColumnName("DetallePendienteVentaId");

        builder.Property(x => x.PendienteVentaId)
            .HasColumnName("PendienteVentaId");

        builder.Property(x => x.ProductoId)
            .HasColumnName("ProductoId");

        builder.Property(x => x.Cantidad)
            .HasColumnName("Cantidad");

        builder.Property(x => x.ValorUnitario)
            .HasColumnName("ValorUnitario")
            .HasPrecision(18, 2);

        builder.Property(x => x.Descuento)
            .HasColumnName("Descuento")
            .HasPrecision(18, 2);

        builder.Property(x => x.Total)
            .HasColumnName("Total")
            .HasPrecision(18, 2);

        builder.HasOne(x => x.PendienteVenta)
            .WithMany(x => x.Detalles)
            .HasForeignKey(x => x.PendienteVentaId);

        builder.HasOne(x => x.Producto)
            .WithMany()
            .HasForeignKey(x => x.ProductoId);
    }
}
