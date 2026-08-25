using Argos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argos.Infrastructure.Persistence.Configurations;

public class OcorrenciaConfiguration : IEntityTypeConfiguration<Ocorrencia>
{
    public void Configure(EntityTypeBuilder<Ocorrencia> builder)
    {
        builder.ToTable("OCORRENCIAS");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Titulo).IsRequired().HasMaxLength(160);
        builder.Property(x => x.Descricao).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.Bairro).HasMaxLength(120);
        builder.Property(x => x.Latitude).IsRequired().HasColumnType("BINARY_DOUBLE");
        builder.Property(x => x.Longitude).IsRequired().HasColumnType("BINARY_DOUBLE");
        builder.Property(x => x.NivelRisco).HasConversion<string>().HasMaxLength(10).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(x => x.DataCriacao).IsRequired();

        builder.HasOne(x => x.Usuario).WithMany()
            .HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.TipoOcorrencia).WithMany()
            .HasForeignKey(x => x.TipoOcorrenciaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ZonaRisco).WithMany()
            .HasForeignKey(x => x.ZonaRiscoId).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(x => x.Comentarios).WithOne(c => c.Ocorrencia)
            .HasForeignKey(c => c.OcorrenciaId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.TipoOcorrenciaId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.UsuarioId);
        builder.HasIndex(x => x.DataCriacao).IsDescending();
    }
}
