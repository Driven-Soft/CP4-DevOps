using Argos.Domain.Entities;
using Argos.Domain.Enums;

namespace Argos.Application.DTOs;

/// <summary>Leitura da auditoria de alertas (opcional).</summary>
public class LogAlertaResponse
{
    public int Id { get; }
    public int AlertaId { get; }
    public int UsuarioId { get; }
    public AcaoLogAlerta Acao { get; }
    public string? DadosAntes { get; }
    public string? DadosDepois { get; }
    public DateTime DataCriacao { get; }

    public LogAlertaResponse(LogAlerta log)
    {
        Id = log.Id;
        AlertaId = log.AlertaId;
        UsuarioId = log.UsuarioId;
        Acao = log.Acao;
        DadosAntes = log.DadosAntes;
        DadosDepois = log.DadosDepois;
        DataCriacao = log.DataCriacao;
    }
}
