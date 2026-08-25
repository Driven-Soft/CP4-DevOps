using Argos.Application.DTOs;
using Argos.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Argos.Api.Controllers;

[ApiController]
[Route("zonas-risco")]
[Produces("application/json")]
[SwaggerTag("Zonas de risco monitoradas (agrupam alertas e ocorrências).")]
public class ZonaRiscoController(IZonaRiscoService service) : ControllerBase
{
    /// <summary>Lista as zonas de risco.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ZonaRiscoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(service.GetAll());

    /// <summary>Obtém uma zona de risco pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ZonaRiscoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var zona = service.GetById(id);
        return zona is null ? NotFound() : Ok(zona);
    }

    /// <summary>Cria uma zona de risco.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ZonaRiscoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] ZonaRiscoRequest request)
    {
        var zona = service.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = zona.Id }, zona);
    }

    /// <summary>Atualiza parcialmente uma zona de risco.</summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(ZonaRiscoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] ZonaRiscoPatchRequest request)
    {
        var zona = service.Update(id, request);
        return zona is null ? NotFound() : Ok(zona);
    }

    /// <summary>Desativa uma zona de risco (soft delete).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id) => service.Delete(id) ? NoContent() : NotFound();
}
