using Argos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argos.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("USUARIOS");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome).IsRequired().HasMaxLength(120);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(180);
        builder.Property(x => x.Senha).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Telefone).HasMaxLength(20);
        builder.Property(x => x.TipoUsuario).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(x => x.DataCriacao).IsRequired();
        builder.Property(x => x.Ativo).IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();
    }
}
