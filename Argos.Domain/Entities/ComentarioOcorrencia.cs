namespace Argos.Domain.Entities;

/// <summary>
/// Mensagem do chat/atualizações dentro do detalhe de uma ocorrência.
/// No DTO, "papel" e "autor" são derivados do <see cref="Usuario"/>.
/// </summary>
public class ComentarioOcorrencia
{
    public int Id { get; private set; }
    public string Mensagem { get; private set; } = null!;
    public int OcorrenciaId { get; private set; }
    public Ocorrencia Ocorrencia { get; private set; } = null!;
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
    public DateTime DataCriacao { get; private set; }
    public bool Ativo { get; private set; }

    public ComentarioOcorrencia(string mensagem, int ocorrenciaId, int usuarioId)
    {
        UpdateMensagem(mensagem);
        OcorrenciaId = ocorrenciaId;
        UsuarioId = usuarioId;
        DataCriacao = DateTime.UtcNow;
        Ativo = true;
    }

    public void UpdateMensagem(string mensagem)
    {
        if (string.IsNullOrWhiteSpace(mensagem))
            throw new ArgumentException("Mensagem é obrigatória");
        if (mensagem.Trim().Length > 1000)
            throw new ArgumentException("Mensagem deve ter no máximo 1000 caracteres");
        Mensagem = mensagem.Trim();
    }

    public void Ativar() => Ativo = true;

    public void Desativar() => Ativo = false;

    protected ComentarioOcorrencia() { } // EF
}
