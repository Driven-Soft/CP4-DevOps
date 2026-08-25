using Argos.Application.DTOs;
using Argos.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Argos.Api.Controllers;

[ApiController]
[Route("tipos-ocorrencia")]
[Produces("application/json")]
[SwaggerTag("Catálogo de tipos de incidente (dropdown e filtros do app).")]
public class TipoOcorrenciaController(ITipoOcorrenciaService service) : ControllerBase
{
    /// <summary>Lista os tipos de ocorrência ativos (com a chave usada pelo app).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<TipoOcorrenciaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(service.GetAll());

    /// <summary>Obtém um tipo de ocorrência pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TipoOcorrenciaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var tipo = service.GetById(id);
        return tipo is null ? NotFound() : Ok(tipo);
    }

    /// <summary>Cria um tipo de ocorrência.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TipoOcorrenciaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] TipoOcorrenciaRequest request)
    {
        var tipo = service.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = tipo.Id }, tipo);
    }

    /// <summary>Atualiza parcialmente um tipo de ocorrência.</summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(TipoOcorrenciaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] TipoOcorrenciaPatchRequest request)
    {
        var tipo = service.Update(id, request);
        return tipo is null ? NotFound() : Ok(tipo);
    }

    /// <summary>Desativa um tipo de ocorrência (soft delete).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id) => service.Delete(id) ? NoContent() : NotFound();
}
