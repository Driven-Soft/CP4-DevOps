using Argos.Application.DTOs;
using Argos.Application.Interfaces.Repositories;
using Argos.Application.Services.Interfaces;

namespace Argos.Application.Services.Implementations;

public class TipoOcorrenciaService(ITipoOcorrenciaRepository repository) : ITipoOcorrenciaService
{
    public TipoOcorrenciaResponse Create(TipoOcorrenciaRequest request)
    {
        var chave = request.Chave.Trim().ToLowerInvariant();
        if (repository.GetByChave(chave) is not null)
            throw new ArgumentException("Já existe um tipo de ocorrência com esta chave");

        var tipo = request.ToDomain();
        repository.Add(tipo);
        repository.SaveChanges();
        return new TipoOcorrenciaResponse(tipo);
    }

    public IReadOnlyCollection<TipoOcorrenciaResponse> GetAll() =>
        repository.ListarAtivos().Select(t => new TipoOcorrenciaResponse(t)).ToList();

    public TipoOcorrenciaResponse? GetById(int id)
    {
        var tipo = repository.GetById(id);
        return tipo is null ? null : new TipoOcorrenciaResponse(tipo);
    }

    public TipoOcorrenciaResponse? Update(int id, TipoOcorrenciaPatchRequest request)
    {
        var tipo = repository.GetById(id);
        if (tipo is null) return null;

        if (request.Nome is not null) tipo.UpdateNome(request.Nome);
        if (request.Descricao is not null) tipo.UpdateDescricao(request.Descricao);
        if (request.Ativo is not null)
        {
            if (request.Ativo.Value) tipo.Ativar();
            else tipo.Desativar();
        }

        repository.Update(tipo);
        repository.SaveChanges();
        return new TipoOcorrenciaResponse(tipo);
    }

    public bool Delete(int id)
    {
        var tipo = repository.GetById(id);
        if (tipo is null) return false;

        tipo.Desativar();
        repository.Update(tipo);
        repository.SaveChanges();
        return true;
    }
}
