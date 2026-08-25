using System.ComponentModel.DataAnnotations;
using Argos.Domain.Entities;
using Argos.Domain.Enums;

namespace Argos.Application.DTOs;

public class ZonaRiscoRequest
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = null!;

    [Required, MaxLength(120)]
    public string Cidade { get; set; } = null!;

    [Required, StringLength(2, MinimumLength = 2)]
    public string Estado { get; set; } = null!;

    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    [MaxLength(40)]
    public string? Regiao { get; set; }

    [MaxLength(300)]
    public string? Descricao { get; set; }

    public NivelRisco NivelRiscoAtual { get; set; } = NivelRisco.BAIXO;

    public ZonaRisco ToDomain() =>
        new(Nome, Cidade, Estado, Latitude, Longitude, Regiao, Descricao, NivelRiscoAtual);
}

public class ZonaRiscoPatchRequest
{
    [MaxLength(120)] public string? Nome { get; set; }
    [MaxLength(40)] public string? Regiao { get; set; }
    [MaxLength(120)] public string? Cidade { get; set; }
    [StringLength(2, MinimumLength = 2)] public string? Estado { get; set; }
    [Range(-90, 90)] public double? Latitude { get; set; }
    [Range(-180, 180)] public double? Longitude { get; set; }
    [MaxLength(300)] public string? Descricao { get; set; }
    public NivelRisco? NivelRiscoAtual { get; set; }
    public bool? Ativa { get; set; }
}

public class ZonaRiscoResponse
{
    public int Id { get; }
    public string Nome { get; }
    public string? Regiao { get; }
    public string Cidade { get; }
    public string Estado { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    public string? Descricao { get; }
    public NivelRisco NivelRiscoAtual { get; }
    public bool Ativa { get; }
    public DateTime DataCriacao { get; }
    public DateTime? AtualizadoEm { get; }

    public ZonaRiscoResponse(ZonaRisco zona)
    {
        Id = zona.Id;
        Nome = zona.Nome;
        Regiao = zona.Regiao;
        Cidade = zona.Cidade;
        Estado = zona.Estado;
        Latitude = zona.Latitude;
        Longitude = zona.Longitude;
        Descricao = zona.Descricao;
        NivelRiscoAtual = zona.NivelRiscoAtual;
        Ativa = zona.Ativa;
        DataCriacao = zona.DataCriacao;
        AtualizadoEm = zona.AtualizadoEm;
    }
}
