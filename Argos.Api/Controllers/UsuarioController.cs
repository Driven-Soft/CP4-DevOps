using Argos.Application.DTOs;
using Argos.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Argos.Api.Controllers;

[ApiController]
[Route("usuarios")]
[Produces("application/json")]
[SwaggerTag("Usuários do app (cadastro local — o POST retorna o id gravado no AsyncStorage).")]
public class UsuarioController(IUsuarioService service) : ControllerBase
{
    /// <summary>Cria um usuário e retorna o id (usado no cadastro do app).</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] UsuarioRequest request)
    {
        var usuario = service.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }

    /// <summary>Lista todos os usuários.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<UsuarioResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(service.GetAll());

    /// <summary>Obtém um usuário pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var usuario = service.GetById(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    /// <summary>Atualiza parcialmente um usuário.</summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] UsuarioPatchRequest request)
    {
        var usuario = service.Update(id, request);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    /// <summary>Desativa um usuário (soft delete).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id) => service.Delete(id) ? NoContent() : NotFound();
}
