using Argos.Application.DTOs;
using Argos.Application.Services.Interfaces;
using Argos.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Argos.Api.Controllers;

[ApiController]
[Route("alertas")]
[Produces("application/json")]
[SwaggerTag("Alertas oficiais por zona de risco (feed e detalhe).")]
public class AlertaController(IAlertaService service) : ControllerBase
{
    /// <summary>
    /// Feed de alertas, filtrável por <c>status</c> (ativo/inativo) e <c>nivel</c>
    /// (BAIXO/MEDIO/ALTO/CRITICO). Sem filtro retorna todos.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<AlertaResponse>), StatusCodes.Status200OK)]
    public IActionResult Buscar([FromQuery] string? status, [FromQuery] NivelRisco? nivel) =>
        Ok(service.Buscar(InterpretarStatus(status), nivel));

    /// <summary>Obtém um alerta pelo id (DTO composto com zona e autor).</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AlertaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var alerta = service.GetById(id);
        return alerta is null ? NotFound() : Ok(alerta);
    }

    /// <summary>Cria um alerta (defesa civil).</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AlertaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] AlertaRequest request)
    {
        var alerta = service.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = alerta.Id }, alerta);
    }

    /// <summary>Atualiza/ativa/desativa um alerta (defesa civil).</summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(AlertaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] AlertaPatchRequest request)
    {
        var alerta = service.Update(id, request);
        return alerta is null ? NotFound() : Ok(alerta);
    }

    /// <summary>Desativa um alerta (soft delete).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id) => service.Delete(id) ? NoContent() : NotFound();

    // Traduz o filtro textual `?status=` do app para o bool? do serviço.
    private static bool? InterpretarStatus(string? status) => status?.Trim().ToLowerInvariant() switch
    {
        "ativo" or "ativos" or "true" or "1" => true,
        "inativo" or "inativos" or "false" or "0" => false,
        _ => null
    };
}
