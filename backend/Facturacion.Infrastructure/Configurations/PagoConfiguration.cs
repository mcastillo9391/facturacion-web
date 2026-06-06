using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configurations;

public class PagoConfiguration
    : IEntityTypeConfiguration<Pago>
{
    public void Configure(
        EntityTypeBuilder<Pago> builder)
    {
        builder.ToTable("pagos");

        builder.HasKey(x => x.IdPago);

        builder.Property(x => x.IdPago)
            .HasColumnName("idpago");

        builder.Property(x => x.IdCli)
            .HasColumnName("idcli");

        builder.Property(x => x.FechaPago)
            .HasColumnName("fechapago");

        builder.Property(x => x.FechaAnulacion)
            .HasColumnName("fecha_anulacion");

        builder.Property(x => x.ValorPago)
            .HasColumnName("valor_pago");

        builder.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.IdCli);

        builder.HasMany(x => x.DetallesPago)
            .WithOne(x => x.Pago)
            .HasForeignKey(x => x.IdPago);
    }
}