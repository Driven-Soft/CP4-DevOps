using Argos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argos.Infrastructure.Persistence.Configurations;

public class ComentarioOcorrenciaConfiguration : IEntityTypeConfiguration<ComentarioOcorrencia>
{
    public void Configure(EntityTypeBuilder<ComentarioOcorrencia> builder)
    {
        builder.ToTable("COMENTARIOS_OCORRENCIA");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Mensagem).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.Ativo).IsRequired();
        builder.Property(x => x.DataCriacao).IsRequired();

        // A relação com Ocorrencia (Cascade) é definida em OcorrenciaConfiguration.
        builder.HasOne(x => x.Usuario).WithMany()
            .HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.OcorrenciaId, x.DataCriacao });
    }
}
