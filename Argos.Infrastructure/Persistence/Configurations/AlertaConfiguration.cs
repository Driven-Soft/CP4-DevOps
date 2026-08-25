using Argos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argos.Infrastructure.Persistence.Configurations;

public class AlertaConfiguration : IEntityTypeConfiguration<Alerta>
{
    public void Configure(EntityTypeBuilder<Alerta> builder)
    {
        builder.ToTable("ALERTAS");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Titulo).IsRequired().HasMaxLength(160);
        builder.Property(x => x.Descricao).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.NivelAlerta).HasConversion<string>().HasMaxLength(10).IsRequired();
        builder.Property(x => x.Ativo).IsRequired();
        builder.Property(x => x.DataCriacao).IsRequired();

        // ZonaRisco obrigatória → Restrict (não cascateia de zona).
        builder.HasOne(x => x.ZonaRisco).WithMany()
            .HasForeignKey(x => x.ZonaRiscoId).OnDelete(DeleteBehavior.Restrict);
        // Autor opcional → SetNull ao apagar o usuário.
        builder.HasOne(x => x.UsuarioCriador).WithMany()
            .HasForeignKey(x => x.UsuarioCriadorId).OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.Ativo);
        builder.HasIndex(x => x.NivelAlerta);
        builder.HasIndex(x => x.ZonaRiscoId);
    }
}
