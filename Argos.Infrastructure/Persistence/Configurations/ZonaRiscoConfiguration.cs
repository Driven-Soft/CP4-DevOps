using Argos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argos.Infrastructure.Persistence.Configurations;

public class ZonaRiscoConfiguration : IEntityTypeConfiguration<ZonaRisco>
{
    public void Configure(EntityTypeBuilder<ZonaRisco> builder)
    {
        builder.ToTable("ZONAS_RISCO");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome).IsRequired().HasMaxLength(120);
        builder.Property(x => x.Regiao).HasMaxLength(40);
        builder.Property(x => x.Cidade).IsRequired().HasMaxLength(120);
        builder.Property(x => x.Estado).IsRequired().HasMaxLength(2).IsFixedLength(); // NCHAR(2)
        builder.Property(x => x.Latitude).IsRequired().HasColumnType("BINARY_DOUBLE");
        builder.Property(x => x.Longitude).IsRequired().HasColumnType("BINARY_DOUBLE");
        builder.Property(x => x.Descricao).HasMaxLength(300);
        builder.Property(x => x.NivelRiscoAtual).HasConversion<string>().HasMaxLength(10).IsRequired();
        builder.Property(x => x.Ativa).IsRequired();
        builder.Property(x => x.DataCriacao).IsRequired();
    }
}
