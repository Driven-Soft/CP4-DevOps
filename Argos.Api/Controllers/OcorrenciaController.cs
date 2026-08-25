using Argos.Application.DTOs;
using Argos.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Argos.Api.Controllers;

[ApiController]
[Route("ocorrencias")]
[Produces("application/json")]
[SwaggerTag("Ocorrências reportadas pelos cidadãos (feed, detalhe, registro e mapa).")]
public class OcorrenciaController(IOcorrenciaService service) : ControllerBase
{
    /// <summary>Feed/busca de ocorrências, filtrável por tipo (chave) e termo de texto.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<OcorrenciaResponse>), StatusCodes.Status200OK)]
    public IActionResult Search([FromQuery] string? tipo, [FromQuery] string? q) =>
        Ok(service.Search(tipo, q));

    /// <summary>Obtém uma ocorrência pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OcorrenciaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var ocorrencia = service.GetById(id);
        return ocorrencia is null ? NotFound() : Ok(ocorrencia);
    }

    /// <summary>Registra uma ocorrência (o autor viaja como usuarioId no corpo).</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OcorrenciaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] OcorrenciaRequest request)
    {
        var ocorrencia = service.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = ocorrencia.Id }, ocorrencia);
    }

    /// <summary>Atualiza status/nível/zona de uma ocorrência (defesa civil).</summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(OcorrenciaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] OcorrenciaPatchRequest request)
    {
        var ocorrencia = service.Update(id, request);
        return ocorrencia is null ? NotFound() : Ok(ocorrencia);
    }

    /// <summary>Remove uma ocorrência (exclusão física; comentários em cascata).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id) => service.Delete(id) ? NoContent() : NotFound();
}
