using Argos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Argos.Infrastructure.Persistence;

/// <summary>
/// Contexto EF Core do Argos (Oracle). Um <see cref="DbSet{TEntity}"/> por entidade;
/// o mapeamento Fluent fica nas <c>IEntityTypeConfiguration</c> da pasta
/// <c>Persistence/Configurations</c>, descobertas automaticamente.
/// </summary>
public class ArgosContext(DbContextOptions<ArgosContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<TipoOcorrencia> TiposOcorrencia => Set<TipoOcorrencia>();
    public DbSet<ZonaRisco> ZonasRisco => Set<ZonaRisco>();
    public DbSet<Alerta> Alertas => Set<Alerta>();
    public DbSet<Ocorrencia> Ocorrencias => Set<Ocorrencia>();
    public DbSet<ComentarioOcorrencia> ComentariosOcorrencia => Set<ComentarioOcorrencia>();
    public DbSet<LogAlerta> LogsAlerta => Set<LogAlerta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArgosContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<bool>().HaveColumnType("NUMBER(1)");
        base.ConfigureConventions(configurationBuilder);
    }
}
