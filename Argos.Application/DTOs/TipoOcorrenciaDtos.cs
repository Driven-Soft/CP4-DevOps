using System.ComponentModel.DataAnnotations;
using Argos.Domain.Entities;

namespace Argos.Application.DTOs;

public class TipoOcorrenciaRequest
{
    [Required, MaxLength(60)]
    public string Nome { get; set; } = null!;

    [Required, MaxLength(40)]
    public string Chave { get; set; } = null!;

    [MaxLength(200)]
    public string? Descricao { get; set; }

    public TipoOcorrencia ToDomain() => new(Nome, Chave, Descricao);
}

public class TipoOcorrenciaPatchRequest
{
    [MaxLength(60)]
    public string? Nome { get; set; }

    [MaxLength(200)]
    public string? Descricao { get; set; }

    public bool? Ativo { get; set; }
}

public class TipoOcorrenciaResponse
{
    public int Id { get; }
    public string Nome { get; }
    public string Chave { get; }
    public string? Descricao { get; }
    public bool Ativo { get; }

    public TipoOcorrenciaResponse(TipoOcorrencia tipo)
    {
        Id = tipo.Id;
        Nome = tipo.Nome;
        Chave = tipo.Chave;
        Descricao = tipo.Descricao;
        Ativo = tipo.Ativo;
    }
}
