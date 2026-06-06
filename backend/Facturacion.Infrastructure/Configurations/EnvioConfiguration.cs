using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configurations;

public class EnvioConfiguration
    : IEntityTypeConfiguration<Envio>
{
    public void Configure(
        EntityTypeBuilder<Envio> builder)
    {
        builder.ToTable("envios");

        builder.HasKey(x => x.Codigo);

        builder.Property(x => x.Codigo)
            .HasColumnName("codigo");

        builder.Property(x => x.FechaEnvio)
            .HasColumnName("fecha_envio");

        builder.Property(x => x.ValorEnvio)
            .HasColumnName("valor_envio");

        builder.Property(x => x.Ciudad)
            .HasColumnName("ciudad")
            .HasMaxLength(200);
    }
}