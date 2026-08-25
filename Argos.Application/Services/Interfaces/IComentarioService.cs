using Argos.Application.DTOs;

namespace Argos.Application.Services.Interfaces;

public interface IComentarioService
{
    IReadOnlyCollection<ComentarioResponse> ListarPorOcorrencia(int ocorrenciaId);
    ComentarioResponse Create(int ocorrenciaId, ComentarioRequest request);
    /// <summary>Soft delete (moderação) — marca <c>Ativo = 0</c>.</summary>
    bool Delete(int id);
}
