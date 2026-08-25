using Argos.Application.Services.Implementations;
using Argos.Application.Services.Interfaces;

namespace Argos.Api.Extensions;

/// <summary>
/// Registro manual dos serviços de aplicação. Cada recurso novo
/// precisa de uma linha aqui.
/// </summary>
public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<ITipoOcorrenciaService, TipoOcorrenciaService>();
        services.AddScoped<IZonaRiscoService, ZonaRiscoService>();
        services.AddScoped<IAlertaService, AlertaService>();
        services.AddScoped<IOcorrenciaService, OcorrenciaService>();
        services.AddScoped<IComentarioService, ComentarioService>();
        return services;
    }
}
