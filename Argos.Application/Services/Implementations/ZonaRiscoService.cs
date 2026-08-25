using Argos.Application.DTOs;
using Argos.Application.Interfaces.Repositories;
using Argos.Application.Services.Interfaces;

namespace Argos.Application.Services.Implementations;

public class ZonaRiscoService(IZonaRiscoRepository repository) : IZonaRiscoService
{
    public ZonaRiscoResponse Create(ZonaRiscoRequest request)
    {
        var zona = request.ToDomain();
        repository.Add(zona);
        repository.SaveChanges();
        return new ZonaRiscoResponse(zona);
    }

    public IReadOnlyCollection<ZonaRiscoResponse> GetAll() =>
        repository.GetAll().Select(z => new ZonaRiscoResponse(z)).ToList();

    public ZonaRiscoResponse? GetById(int id)
    {
        var zona = repository.GetById(id);
        return zona is null ? null : new ZonaRiscoResponse(zona);
    }

    public ZonaRiscoResponse? Update(int id, ZonaRiscoPatchRequest request)
    {
        var zona = repository.GetById(id);
        if (zona is null) return null;

        if (request.Nome is not null) zona.UpdateNome(request.Nome);
        if (request.Regiao is not null) zona.UpdateRegiao(request.Regiao);
        if (request.Cidade is not null) zona.UpdateCidade(request.Cidade);
        if (request.Estado is not null) zona.UpdateEstado(request.Estado);
        if (request.Latitude is not null || request.Longitude is not null)
            zona.UpdateCoordenadas(request.Latitude ?? zona.Latitude, request.Longitude ?? zona.Longitude);
        if (request.Descricao is not null) zona.UpdateDescricao(request.Descricao);
        if (request.NivelRiscoAtual is not null) zona.AlterarNivelRisco(request.NivelRiscoAtual.Value);
        if (request.Ativa is not null)
        {
            if (request.Ativa.Value) zona.Ativar();
            else zona.Desativar();
        }

        repository.Update(zona);
        repository.SaveChanges();
        return new ZonaRiscoResponse(zona);
    }

    public bool Delete(int id)
    {
        var zona = repository.GetById(id);
        if (zona is null) return false;

        zona.Desativar();
        repository.Update(zona);
        repository.SaveChanges();
        return true;
    }
}
