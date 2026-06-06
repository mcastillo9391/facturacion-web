using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configurations;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("producto");

        builder.HasKey(x => x.IdProducto);

        builder.Property(x => x.IdProducto)
            .HasColumnName("idproducto");

        builder.Property(x => x.Nombre)
            .HasColumnName("producto")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.ValorCosto)
            .HasColumnName("valorcosto")
            .HasPrecision(18, 2);

        builder.Property(x => x.ValorVenta)
            .HasColumnName("valorventa")
            .HasPrecision(18, 2);

        builder.Property(x => x.Activo)
            .HasColumnName("activo");
    }
}