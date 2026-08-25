using Argos.Application.DTOs;
using Argos.Application.Interfaces.Repositories;
using Argos.Application.Services.Interfaces;

namespace Argos.Application.Services.Implementations;

public class OcorrenciaService(IOcorrenciaRepository repository) : IOcorrenciaService
{
    public OcorrenciaResponse Create(OcorrenciaRequest request)
    {
        var ocorrencia = request.ToDomain();
        repository.Add(ocorrencia);
        repository.SaveChanges();
        // Recarrega com TipoOcorrencia para o DTO expor `tipo = Chave`.
        return new OcorrenciaResponse(repository.GetByIdComTipo(ocorrencia.Id)!);
    }

    public IReadOnlyCollection<OcorrenciaResponse> Search(string? tipoChave, string? termo) =>
        repository.Search(tipoChave, termo).Select(o => new OcorrenciaResponse(o)).ToList();

    public OcorrenciaResponse? GetById(int id)
    {
        var ocorrencia = repository.GetByIdComTipo(id);
        return ocorrencia is null ? null : new OcorrenciaResponse(ocorrencia);
    }

    public OcorrenciaResponse? Update(int id, OcorrenciaPatchRequest request)
    {
        var ocorrencia = repository.GetById(id);
        if (ocorrencia is null) return null;

        if (request.NivelRisco is not null) ocorrencia.AlterarNivelRisco(request.NivelRisco.Value);
        if (request.ZonaRiscoId is not null) ocorrencia.VincularZona(request.ZonaRiscoId);
        if (request.Status is not null) ocorrencia.AlterarStatus(request.Status.Value);

        repository.Update(ocorrencia);
        repository.SaveChanges();
        return new OcorrenciaResponse(repository.GetByIdComTipo(ocorrencia.Id)!);
    }

    public bool Delete(int id)
    {
        var ocorrencia = repository.GetById(id);
        if (ocorrencia is null) return false;

        // Ocorrência não tem soft delete — exclusão física;
        // os comentários caem em cascata.
        repository.Delete(ocorrencia);
        repository.SaveChanges();
        return true;
    }
}
