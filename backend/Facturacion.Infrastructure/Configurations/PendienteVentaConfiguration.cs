using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configurations;

public class PendienteVentaConfiguration
    : IEntityTypeConfiguration<PendienteVenta>
{
    public void Configure(
        EntityTypeBuilder<PendienteVenta> builder)
    {
        builder.ToTable("PendientesVenta");

        builder.HasKey(x => x.PendienteVentaId);

        builder.Property(x => x.PendienteVentaId)
            .HasColumnName("PendienteVentaId");

        builder.Property(x => x.ClienteId)
            .HasColumnName("ClienteId");

        builder.Property(x => x.UsuarioId)
            .HasColumnName("UsuarioId");

        builder.Property(x => x.UsuarioAsig)
            .HasColumnName("UsuarioAsig");

        builder.Property(x => x.FechaCreacion)
            .HasColumnName("FechaCreacion");

        builder.Property(x => x.FechaActualizacion)
            .HasColumnName("FechaActualizacion");

        builder.Property(x => x.Estado)
            .HasColumnName("Estado")
            .HasMaxLength(20);

        builder.Property(x => x.Observaciones)
            .HasColumnName("Observaciones")
            .HasMaxLength(500);

        builder.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.ClienteId);

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId);

        builder.HasOne(x => x.UsuarioAsigNavigation)
            .WithMany()
            .HasForeignKey(x => x.UsuarioAsig)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
