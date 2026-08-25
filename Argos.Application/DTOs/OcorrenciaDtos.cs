using System.ComponentModel.DataAnnotations;
using Argos.Domain.Entities;
using Argos.Domain.Enums;

namespace Argos.Application.DTOs;

public class OcorrenciaRequest
{
    [Required, MaxLength(160)]
    public string Titulo { get; set; } = null!;

    [Required, MaxLength(2000)]
    public string Descricao { get; set; } = null!;

    [Required]
    public int TipoOcorrenciaId { get; set; }

    /// <summary>Autor da ocorrência (não há JWT — o id viaja no corpo).</summary>
    [Required]
    public int UsuarioId { get; set; }

    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    public NivelRisco NivelRisco { get; set; } = NivelRisco.MEDIO;

    public int? ZonaRiscoId { get; set; }

    [MaxLength(120)]
    public string? Bairro { get; set; }

    public Ocorrencia ToDomain() =>
        new(Titulo, Descricao, TipoOcorrenciaId, UsuarioId, Latitude, Longitude,
            NivelRisco, ZonaRiscoId, Bairro);
}

/// <summary>PATCH da ocorrência — principalmente mudança de <c>Status</c> (defesa civil).</summary>
public class OcorrenciaPatchRequest
{
    public StatusOcorrencia? Status { get; set; }
    public NivelRisco? NivelRisco { get; set; }
    public int? ZonaRiscoId { get; set; }
}

/// <summary>DTO de leitura: <c>tipo</c> vem de <see cref="TipoOcorrencia.Chave"/>.</summary>
public class OcorrenciaResponse
{
    public int Id { get; }
    public string Titulo { get; }
    public string Tipo { get; }
    public NivelRisco NivelRisco { get; }
    public StatusOcorrencia Status { get; }
    public string Descricao { get; }
    public string? Bairro { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    public DateTime DataCriacao { get; }

    public OcorrenciaResponse(Ocorrencia ocorrencia)
    {
        Id = ocorrencia.Id;
        Titulo = ocorrencia.Titulo;
        Tipo = ocorrencia.TipoOcorrencia?.Chave ?? string.Empty;
        NivelRisco = ocorrencia.NivelRisco;
        Status = ocorrencia.Status;
        Descricao = ocorrencia.Descricao;
        Bairro = ocorrencia.Bairro;
        Latitude = ocorrencia.Latitude;
        Longitude = ocorrencia.Longitude;
        DataCriacao = ocorrencia.DataCriacao;
    }
}
