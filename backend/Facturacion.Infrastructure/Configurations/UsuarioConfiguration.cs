using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuario");

        builder.HasKey(x => x.UsuarioId);

        builder.Property(x => x.UsuarioId)
            .HasColumnName("usuarioid");

        builder.Property(x => x.RolId)
            .HasColumnName("rolid");

        builder.Property(x => x.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Username)
            .HasColumnName("username")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasColumnName("PasswordHash")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Activo)
            .HasColumnName("activo")
            .IsRequired();

        builder.HasOne(x => x.Rol)
            .WithMany(x => x.Usuarios)
            .HasForeignKey(x => x.RolId);
    }
}