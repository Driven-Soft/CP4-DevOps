using Argos.Application.Interfaces.Repositories;
using Argos.Infrastructure.Persistence;
using Argos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Argos.Api.Extensions;

/// <summary>
/// Registro manual dos repositórios e do <see cref="ArgosContext"/> (MySQL).
/// A connection string vem de <c>ConnectionStrings:ArgosMySql</c>.
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
        var connectionString = configuration.GetConnectionString("ArgosMySql")
            ?? throw new InvalidOperationException("Connection string 'ArgosMySql' não configurada.");

        services.AddDbContext<ArgosContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));
        return services;
    }
}
