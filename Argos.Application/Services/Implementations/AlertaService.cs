using System.Text.Json;
using Argos.Application.DTOs;
using Argos.Application.Interfaces.Repositories;
using Argos.Application.Services.Interfaces;
using Argos.Domain.Entities;
using Argos.Domain.Enums;

namespace Argos.Application.Services.Implementations;

public class AlertaService(IAlertaRepository repository, ILogAlertaRepository logRepository) : IAlertaService
{
    public AlertaResponse Create(AlertaRequest request)
    {
        var alerta = request.ToDomain();
        repository.Add(alerta);
        repository.SaveChanges();

        RegistrarLog(alerta, AcaoLogAlerta.CRIADO, alerta.UsuarioCriadorId, antes: null, depois: Snapshot(alerta));

        // Recarrega com ZonaRisco + UsuarioCriador para o DTO composto.
        return new AlertaResponse(repository.GetByIdCompleto(alerta.Id)!);
    }

    public IReadOnlyCollection<AlertaResponse> Buscar(bool? apenasAtivos, NivelRisco? nivel) =>
        repository.Buscar(apenasAtivos, nivel).Select(a => new AlertaResponse(a)).ToList();

    public AlertaResponse? GetById(int id)
    {
        var alerta = repository.GetByIdCompleto(id);
        return alerta is null ? null : new AlertaResponse(alerta);
    }

    public AlertaResponse? Update(int id, AlertaPatchRequest request)
    {
        var alerta = repository.GetById(id);
        if (alerta is null) return null;

        var antes = Snapshot(alerta);

        if (request.Titulo is not null) alerta.UpdateTitulo(request.Titulo);
        if (request.Descricao is not null) alerta.UpdateDescricao(request.Descricao);
        if (request.NivelAlerta is not null) alerta.AlterarNivel(request.NivelAlerta.Value);
        if (request.InicioVigencia is not null || request.FimVigencia is not null)
            alerta.DefinirVigencia(request.InicioVigencia ?? alerta.InicioVigencia,
                                   request.FimVigencia ?? alerta.FimVigencia);

        // Mudança de ativação vira ação específica na auditoria; o resto é EDITADO.
        AcaoLogAlerta acao = AcaoLogAlerta.EDITADO;
        if (request.Ativo is not null && request.Ativo.Value != alerta.Ativo)
        {
            if (request.Ativo.Value) { alerta.Ativar(); acao = AcaoLogAlerta.ATIVADO; }
            else { alerta.Desativar(); acao = AcaoLogAlerta.DESATIVADO; }
        }

        repository.Update(alerta);
        repository.SaveChanges();

        RegistrarLog(alerta, acao, request.UsuarioId, antes, Snapshot(alerta));

        return new AlertaResponse(repository.GetByIdCompleto(alerta.Id)!);
    }

    public bool Delete(int id)
    {
        var alerta = repository.GetById(id);
        if (alerta is null) return false;

        // Soft delete: "encerra" o alerta marcando Ativo = 0.
        alerta.Desativar();
        repository.Update(alerta);
        repository.SaveChanges();
        return true;
    }

    /// <summary>
    /// Grava a auditoria apenas quando há autor identificado
    /// (<c>LogAlerta.UsuarioId</c> é obrigatório). Sem ator → não registra.
    /// </summary>
    private void RegistrarLog(Alerta alerta, AcaoLogAlerta acao, int? usuarioId, string? antes, string? depois)
    {
        if (usuarioId is null) return;
        logRepository.Add(new LogAlerta(alerta.Id, usuarioId.Value, acao, antes, depois));
        logRepository.SaveChanges();
    }

    private static string Snapshot(Alerta alerta) => JsonSerializer.Serialize(new
    {
        alerta.Titulo,
        alerta.Descricao,
        alerta.NivelAlerta,
        alerta.ZonaRiscoId,
        alerta.InicioVigencia,
        alerta.FimVigencia,
        alerta.Ativo
    });
}
