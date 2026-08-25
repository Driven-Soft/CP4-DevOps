using Argos.Application.DTOs;

namespace Argos.Application.Services.Interfaces;

public interface IOcorrenciaService
{
    OcorrenciaResponse Create(OcorrenciaRequest request);
    /// <summary>Feed/busca (<c>?tipo=&amp;q=</c>).</summary>
    IReadOnlyCollection<OcorrenciaResponse> Search(string? tipoChave, string? termo);
    OcorrenciaResponse? GetById(int id);
    OcorrenciaResponse? Update(int id, OcorrenciaPatchRequest request);
    bool Delete(int id);
}
