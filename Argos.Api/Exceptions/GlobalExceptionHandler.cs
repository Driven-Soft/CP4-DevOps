using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace Argos.Api.Exceptions;

/// <summary>
/// Tratamento global de erros → <c>ProblemDetails</c> (RFC 7807). O <c>Detail</c>
/// só é exposto em Development:
/// <list type="bullet">
/// <item><see cref="ArgumentException"/>/<see cref="InvalidOperationException"/> → 400</item>
/// <item><see cref="KeyNotFoundException"/> → 404</item>
/// <item><see cref="UnauthorizedAccessException"/> → 401</item>
/// <item><see cref="OracleException"/> → 500 ("Banco de dados indisponível")</item>
/// </list>
/// </summary>
public class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, title) = Mapear(exception);
        httpContext.Response.StatusCode = status;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = environment.IsDevelopment() ? exception.Message : null
            }
        });
    }

    // ArgumentNullException herda de ArgumentException, então já cai no primeiro caso.
    private static (int Status, string Title) Mapear(Exception exception) => exception switch
    {
        ArgumentException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
        InvalidOperationException => (StatusCodes.Status400BadRequest, "Operação inválida"),
        KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
        UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Não autorizado"),
        OracleException => (StatusCodes.Status500InternalServerError, "Banco de dados indisponível"),
        _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor")
    };
}
