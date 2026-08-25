using Argos.Domain.Enums;

namespace Argos.Domain.Entities;

/// <summary>
/// Aviso oficial vinculado a uma zona de risco. Alimenta o feed e o detalhe
/// de alertas do app. O autor (<see cref="UsuarioCriadorId"/>) é opcional.
/// </summary>
public class Alerta
{
    public int Id { get; private set; }
    public string Titulo { get; private set; } = null!;
    public string Descricao { get; private set; } = null!;
    public NivelRisco NivelAlerta { get; private set; }
    public int ZonaRiscoId { get; private set; }
    public ZonaRisco ZonaRisco { get; private set; } = null!;
    public int? UsuarioCriadorId { get; private set; }
    public Usuario? UsuarioCriador { get; private set; }
    public DateTime? InicioVigencia { get; private set; }
    public DateTime? FimVigencia { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime DataCriacao { get; private set; }

    public Alerta(string titulo, string descricao, NivelRisco nivelAlerta,
        int zonaRiscoId, int? usuarioCriadorId = null,
        DateTime? inicioVigencia = null, DateTime? fimVigencia = null)
    {
        UpdateTitulo(titulo);
        UpdateDescricao(descricao);
        NivelAlerta = nivelAlerta;
        ZonaRiscoId = zonaRiscoId;
        UsuarioCriadorId = usuarioCriadorId;
        DefinirVigencia(inicioVigencia, fimVigencia);
        Ativo = true;
        DataCriacao = DateTime.UtcNow;
    }

    public void UpdateTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título é obrigatório");
        if (titulo.Trim().Length > 160)
            throw new ArgumentException("Título deve ter no máximo 160 caracteres");
        Titulo = titulo.Trim();
    }

    public void UpdateDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição é obrigatória");
        if (descricao.Trim().Length > 2000)
            throw new ArgumentException("Descrição deve ter no máximo 2000 caracteres");
        Descricao = descricao.Trim();
    }

    public void AlterarNivel(NivelRisco nivelAlerta) => NivelAlerta = nivelAlerta;

    public void DefinirVigencia(DateTime? inicioVigencia, DateTime? fimVigencia)
    {
        if (inicioVigencia.HasValue && fimVigencia.HasValue && fimVigencia < inicioVigencia)
            throw new ArgumentException("Fim de vigência não pode ser anterior ao início");
        InicioVigencia = inicioVigencia;
        FimVigencia = fimVigencia;
    }

    public void Ativar() => Ativo = true;

    public void Desativar() => Ativo = false;

    protected Alerta() { } // EF
}
