using Argos.Application.DTOs;
using Argos.Application.Interfaces.Repositories;
using Argos.Application.Services.Interfaces;

namespace Argos.Application.Services.Implementations;

public class ComentarioService(IComentarioOcorrenciaRepository repository) : IComentarioService
{
    public IReadOnlyCollection<ComentarioResponse> ListarPorOcorrencia(int ocorrenciaId) =>
        repository.ListarPorOcorrencia(ocorrenciaId).Select(c => new ComentarioResponse(c)).ToList();

    public ComentarioResponse Create(int ocorrenciaId, ComentarioRequest request)
    {
        var comentario = request.ToDomain(ocorrenciaId);
        repository.Add(comentario);
        repository.SaveChanges();
        // Recarrega com o autor para o DTO derivar `autor`/`papel`.
        return new ComentarioResponse(repository.GetByIdComUsuario(comentario.Id) ?? comentario);
    }

    public bool Delete(int id)
    {
        var comentario = repository.GetById(id);
        if (comentario is null) return false;

        // Soft delete.
        comentario.Desativar();
        repository.Update(comentario);
        repository.SaveChanges();
        return true;
    }
}
