using Argos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argos.Infrastructure.Persistence.Configurations;

public class LogAlertaConfiguration : IEntityTypeConfiguration<LogAlerta>
{
    public void Configure(EntityTypeBuilder<LogAlerta> builder)
    {
        builder.ToTable("LOGS_ALERTA");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Acao).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(x => x.DadosAntes).HasColumnType("CLOB");
        builder.Property(x => x.DadosDepois).HasColumnType("CLOB");
        builder.Property(x => x.DataCriacao).IsRequired();

        builder.HasOne(x => x.Alerta).WithMany()
            .HasForeignKey(x => x.AlertaId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Usuario).WithMany()
            .HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Restrict);
    }
}
