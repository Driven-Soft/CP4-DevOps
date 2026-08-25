namespace Argos.Domain.Entities;

/// <summary>
/// Catálogo dos tipos de incidente (dropdown e filtros do app).
/// A <see cref="Chave"/> é o vínculo estável com o app (ex.: "alagamento").
/// </summary>
public class TipoOcorrencia
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Chave { get; private set; } = null!;
    public string? Descricao { get; private set; }
    public bool Ativo { get; private set; }

    public TipoOcorrencia(string nome, string chave, string? descricao = null)
    {
        UpdateNome(nome);
        UpdateChave(chave);
        UpdateDescricao(descricao);
        Ativo = true;
    }

    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório");
        if (nome.Trim().Length > 60)
            throw new ArgumentException("Nome deve ter no máximo 60 caracteres");
        Nome = nome.Trim();
    }

    public void UpdateChave(string chave)
    {
        if (string.IsNullOrWhiteSpace(chave))
            throw new ArgumentException("Chave é obrigatória");
        var slug = chave.Trim().ToLowerInvariant();
        if (slug.Length > 40)
            throw new ArgumentException("Chave deve ter no máximo 40 caracteres");
        Chave = slug;
    }

    public void UpdateDescricao(string? descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
        {
            Descricao = null;
            return;
        }
        if (descricao.Trim().Length > 200)
            throw new ArgumentException("Descrição deve ter no máximo 200 caracteres");
        Descricao = descricao.Trim();
    }

    public void Ativar() => Ativo = true;

    public void Desativar() => Ativo = false;

    protected TipoOcorrencia() { } // EF
}
