using Argos.Domain.Enums;

namespace Argos.Domain.Entities;

/// <summary>
/// Auditoria (opcional) das ações sobre um alerta. Guarda o estado antes/depois
/// em JSON (CLOB) para rastrear criação, edição e mudanças de ativação.
/// </summary>
public class LogAlerta
{
    public int Id { get; private set; }
    public int AlertaId { get; private set; }
    public Alerta Alerta { get; private set; } = null!;
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
    public AcaoLogAlerta Acao { get; private set; }
    public string? DadosAntes { get; private set; }
    public string? DadosDepois { get; private set; }
    public DateTime DataCriacao { get; private set; }

    public LogAlerta(int alertaId, int usuarioId, AcaoLogAlerta acao,
        string? dadosAntes = null, string? dadosDepois = null)
    {
        AlertaId = alertaId;
        UsuarioId = usuarioId;
        Acao = acao;
        DadosAntes = dadosAntes;
        DadosDepois = dadosDepois;
        DataCriacao = DateTime.UtcNow;
    }

    protected LogAlerta() { } // EF
}
