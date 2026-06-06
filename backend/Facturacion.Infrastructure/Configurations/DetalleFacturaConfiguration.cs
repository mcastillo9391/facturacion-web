using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configurations;

public class DetalleFacturaConfiguration
    : IEntityTypeConfiguration<DetalleFactura>
{
    public void Configure(
        EntityTypeBuilder<DetalleFactura> builder)
    {
        builder.ToTable("detalle_factura");

        builder.HasKey(x => x.Codigo);

        builder.Property(x => x.Codigo)
            .HasColumnName("codigo");

        builder.Property(x => x.Producto)
            .HasColumnName("producto");

        builder.Property(x => x.Factura)
            .HasColumnName("factura");

        builder.Property(x => x.CostoUnitario)
            .HasColumnName("costo_unitario");

        builder.Property(x => x.ValorUnitario)
            .HasColumnName("valor_unitario");

        builder.Property(x => x.Descuento)
            .HasColumnName("descuento");

        builder.Property(x => x.Cantidad)
            .HasColumnName("cantidad");

        builder.Property(x => x.Total)
            .HasColumnName("total");

        builder.HasOne(x => x.ProductoNavigation)
            .WithMany()
            .HasForeignKey(x => x.Producto);

        builder.HasOne(x => x.FacturaNavigation)
            .WithMany(x => x.Detalles)
            .HasForeignKey(x => x.Factura);
    }
}