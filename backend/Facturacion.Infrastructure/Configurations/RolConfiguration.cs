using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configurations;

public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("rol");

        builder.HasKey(x => x.RolId);

        builder.Property(x => x.RolId)
            .HasColumnName("rolid");

        builder.Property(x => x.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired();
    }
}