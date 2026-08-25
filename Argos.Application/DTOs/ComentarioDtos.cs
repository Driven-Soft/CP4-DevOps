using System.ComponentModel.DataAnnotations;
using Argos.Domain.Entities;
using Argos.Domain.Enums;

namespace Argos.Application.DTOs;

/// <summary>
/// Corpo do <c>POST /ocorrencias/{ocorrenciaId}/comentarios</c>.
/// O <c>OcorrenciaId</c> vem da rota; o <c>UsuarioId</c> (autor) vem do corpo.
/// </summary>
public class ComentarioRequest
{
    [Required, MaxLength(1000)]
    public string Mensagem { get; set; } = null!;

    [Required]
    public int UsuarioId { get; set; }

    public ComentarioOcorrencia ToDomain(int ocorrenciaId) =>
        new(Mensagem, ocorrenciaId, UsuarioId);
}

/// <summary>
/// DTO composto: <c>autor</c> e <c>papel</c> derivam de <see cref="ComentarioOcorrencia.Usuario"/>.
/// Exige a entidade com a navegação <c>Usuario</c> carregada.
/// </summary>
public class ComentarioResponse
{
    public int Id { get; }
    public string Autor { get; }
    public string Papel { get; }
    public string Texto { get; }
    public DateTime DataCriacao { get; }

    public ComentarioResponse(ComentarioOcorrencia comentario)
    {
        Id = comentario.Id;
        Autor = comentario.Usuario?.Nome ?? string.Empty;
        Papel = comentario.Usuario?.TipoUsuario == TipoUsuario.DEFESA_CIVIL ? "defesa_civil" : "cidadao";
        Texto = comentario.Mensagem;
        DataCriacao = comentario.DataCriacao;
    }
}
