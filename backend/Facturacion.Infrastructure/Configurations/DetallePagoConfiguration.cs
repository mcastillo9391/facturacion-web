using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configurations;

public class DetallePagoConfiguration
    : IEntityTypeConfiguration<DetallePago>
{
    public void Configure(
        EntityTypeBuilder<DetallePago> builder)
    {
        builder.ToTable("detalle_pago");

        builder.HasKey(x => x.Codigo);

        builder.Property(x => x.Codigo)
            .HasColumnName("codigo");

        builder.Property(x => x.IdPago)
            .HasColumnName("idpago");

        builder.Property(x => x.Factura)
            .HasColumnName("factura");

        builder.Property(x => x.ValorPago)
            .HasColumnName("valor_pago");

        builder.HasOne(x => x.Pago)
            .WithMany(x => x.DetallesPago)
            .HasForeignKey(x => x.IdPago);

        builder.HasOne(x => x.FacturaNavigation)
            .WithMany(x => x.DetallesPago)
            .HasForeignKey(x => x.Factura);
    }
}