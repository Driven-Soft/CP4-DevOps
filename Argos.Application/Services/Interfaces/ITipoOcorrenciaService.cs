using Argos.Application.DTOs;

namespace Argos.Application.Services.Interfaces;

public interface ITipoOcorrenciaService
{
    TipoOcorrenciaResponse Create(TipoOcorrenciaRequest request);
    /// <summary>Lista os tipos ativos (dropdown/filtros do app).</summary>
    IReadOnlyCollection<TipoOcorrenciaResponse> GetAll();
    TipoOcorrenciaResponse? GetById(int id);
    TipoOcorrenciaResponse? Update(int id, TipoOcorrenciaPatchRequest request);
    bool Delete(int id);
}
