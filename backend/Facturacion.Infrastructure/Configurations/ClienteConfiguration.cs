using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(x => x.IdCli);

        builder.Property(x => x.IdCli)
            .HasColumnName("idcli");

        builder.Property(x => x.Identificacion)
            .HasColumnName("identificacion")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Direccion)
            .HasColumnName("direccion")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Telefono)
            .HasColumnName("telefono")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.FechaCreacion)
            .HasColumnName("FechaCreacion");

        builder.Property(x => x.Activo)
            .HasColumnName("activo");

        builder.HasIndex(x => x.Identificacion)
            .IsUnique();
    }
}