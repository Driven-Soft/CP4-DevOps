using Argos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argos.Infrastructure.Persistence.Configurations;

public class TipoOcorrenciaConfiguration : IEntityTypeConfiguration<TipoOcorrencia>
{
    public void Configure(EntityTypeBuilder<TipoOcorrencia> builder)
    {
        builder.ToTable("TIPOS_OCORRENCIA");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome).IsRequired().HasMaxLength(60);
        builder.Property(x => x.Chave).IsRequired().HasMaxLength(40);
        builder.Property(x => x.Descricao).HasMaxLength(200);
        builder.Property(x => x.Ativo).IsRequired();

        builder.HasIndex(x => x.Chave).IsUnique();
    }
}
