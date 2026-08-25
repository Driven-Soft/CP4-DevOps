using Argos.Application.DTOs;
using Argos.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Argos.Api.Controllers;

[ApiController]
[Route("ocorrencias/{ocorrenciaId:int}/comentarios")]
[Produces("application/json")]
[SwaggerTag("Comentários/atualizações dentro do detalhe de uma ocorrência.")]
public class ComentarioController(IComentarioService service) : ControllerBase
{
    /// <summary>Lista os comentários ativos de uma ocorrência (autor/papel derivados do usuário).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ComentarioResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll(int ocorrenciaId) =>
        Ok(service.ListarPorOcorrencia(ocorrenciaId));

    /// <summary>Adiciona um comentário (a ocorrência vem da rota; o autor, do corpo).</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ComentarioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create(int ocorrenciaId, [FromBody] ComentarioRequest request)
    {
        var comentario = service.Create(ocorrenciaId, request);
        return CreatedAtAction(nameof(GetAll), new { ocorrenciaId }, comentario);
    }

    /// <summary>Remove um comentário (soft delete — moderação).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int ocorrenciaId, int id) =>
        service.Delete(id) ? NoContent() : NotFound();
}
