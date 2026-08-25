using Argos.Application.Interfaces.Repositories;
using Argos.Infrastructure.Persistence;
using Argos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Argos.Api.Extensions;

/// <summary>
/// Registro manual dos repositórios e do <see cref="ArgosContext"/> (Oracle).
/// A connection string vem de <c>ConnectionStrings:ArgosOracle</c>.
/// </summary>
public static class PersistenceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ITipoOcorrenciaRepository, TipoOcorrenciaRepository>();
        services.AddScoped<IZonaRiscoRepository, ZonaRiscoRepository>();
        services.AddScoped<IAlertaRepository, AlertaRepository>();
        services.AddScoped<IOcorrenciaRepository, OcorrenciaRepository>();
        services.AddScoped<IComentarioOcorrenciaRepository, ComentarioOcorrenciaRepository>();
        services.AddScoped<ILogAlertaRepository, LogAlertaRepository>();
        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ArgosOracle")
            ?? throw new InvalidOperationException("Connection string 'ArgosOracle' não configurada.");

        services.AddDbContext<ArgosContext>(options =>
            options.UseOracle(connectionString, oracle =>
                oracle.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19)));
        return services;
    }
}
