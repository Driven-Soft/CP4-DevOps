using System.Reflection;
using Microsoft.OpenApi;

namespace Argos.Api.Extensions;

/// <summary>
/// Swagger via Swashbuckle.Annotations, com os comentários XML do projeto.
/// </summary>
public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Argos API",
                Version = "v1",
                Description = "API de monitoramento de ocorrências, alertas e zonas de risco do app Argos."
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);
        });

        return services;
    }
}
