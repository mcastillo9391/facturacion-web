using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configurations;

public class FacturaConfiguration
    : IEntityTypeConfiguration<Factura>
{
    public void Configure(
        EntityTypeBuilder<Factura> builder)
    {
        builder.ToTable("factura");

        builder.HasKey(x => x.Codigo);

        builder.Property(x => x.Codigo)
            .HasColumnName("codigo");

        builder.Property(x => x.IdCli)
            .HasColumnName("idcli");

        builder.Property(x => x.FechaGen)
            .HasColumnName("fechagen");

        builder.Property(x => x.ValorFactura)
            .HasColumnName("valorfactura")
            .HasPrecision(18, 2);

        builder.Property(x => x.ValorAbonado)
            .HasColumnName("valor_abonado")
            .HasPrecision(18, 2);

        builder.Property(x => x.UsuarioId)
            .HasColumnName("usuarioid");
        
        builder.Property(x => x.UsuarioAsig)
            .HasColumnName("usuarioasig")
            .IsRequired(false);

        builder.Property(x => x.Estado)
            .HasColumnName("estado")
            .HasMaxLength(20);

        builder.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.IdCli);

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