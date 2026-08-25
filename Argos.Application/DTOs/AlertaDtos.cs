using System.ComponentModel.DataAnnotations;
using Argos.Domain.Entities;
using Argos.Domain.Enums;

namespace Argos.Application.DTOs;

public class AlertaRequest
{
    [Required, MaxLength(160)]
    public string Titulo { get; set; } = null!;

    [Required, MaxLength(2000)]
    public string Descricao { get; set; } = null!;

    public NivelRisco NivelAlerta { get; set; }

    [Required]
    public int ZonaRiscoId { get; set; }

    /// <summary>Autor (defesa civil); opcional — quando ausente o DTO mostra "Defesa Civil".</summary>
    public int? UsuarioCriadorId { get; set; }

    public DateTime? InicioVigencia { get; set; }
    public DateTime? FimVigencia { get; set; }

    public Alerta ToDomain() =>
        new(Titulo, Descricao, NivelAlerta, ZonaRiscoId, UsuarioCriadorId, InicioVigencia, FimVigencia);
}

/// <summary>PATCH do alerta — cobre principalmente a ativação/desativação (defesa civil).</summary>
public class AlertaPatchRequest
{
    [MaxLength(160)] public string? Titulo { get; set; }
    [MaxLength(2000)] public string? Descricao { get; set; }
    public NivelRisco? NivelAlerta { get; set; }
    public DateTime? InicioVigencia { get; set; }
    public DateTime? FimVigencia { get; set; }
    public bool? Ativo { get; set; }

    /// <summary>Quem executa a ação — registrado na auditoria (<c>LogAlerta</c>) quando informado.</summary>
    public int? UsuarioId { get; set; }
}

/// <summary>
/// DTO composto: combina <see cref="Alerta"/> + <see cref="ZonaRisco"/> + autor.
/// Exige a entidade com as navegações carregadas (<c>Include</c>).
/// </summary>
public class AlertaResponse
{
    public int Id { get; }
    public string Titulo { get; }
    public NivelRisco NivelAlerta { get; }
    public string? Zona { get; }
    public string Localizacao { get; }
    public string Autor { get; }
    public string Descricao { get; }
    public bool Ativo { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    public DateTime DataCriacao { get; }
    public DateTime? FimVigencia { get; }

    public AlertaResponse(Alerta alerta)
    {
        Id = alerta.Id;
        Titulo = alerta.Titulo;
        NivelAlerta = alerta.NivelAlerta;
        Zona = alerta.ZonaRisco?.Regiao;
        Localizacao = alerta.ZonaRisco?.Nome ?? string.Empty;
        Autor = alerta.UsuarioCriador?.Nome ?? "Defesa Civil";
        Descricao = alerta.Descricao;
        Ativo = alerta.Ativo;
        Latitude = alerta.ZonaRisco?.Latitude ?? 0;
        Longitude = alerta.ZonaRisco?.Longitude ?? 0;
        DataCriacao = alerta.DataCriacao;
        FimVigencia = alerta.FimVigencia;
    }
}
