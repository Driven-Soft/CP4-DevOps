using Argos.Domain.Enums;

namespace Argos.Domain.Entities;

/// <summary>
/// Identifica quem usa o app; autor de ocorrências, comentários e alertas.
/// Cadastro/login são locais (AsyncStorage); a senha é texto validado (mín. 6),
/// nunca exposta em DTO — sem hash e sem login no servidor.
/// </summary>
public class Usuario
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Senha { get; private set; } = null!;
    public string? Telefone { get; private set; }
    public TipoUsuario TipoUsuario { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime? UltimoAcessoEm { get; private set; }

    public Usuario(string nome, string email, string senha,
        string? telefone = null, TipoUsuario tipoUsuario = TipoUsuario.CIDADAO)
    {
        UpdateNome(nome);
        UpdateEmail(email);
        UpdateSenha(senha);
        UpdateTelefone(telefone);
        TipoUsuario = tipoUsuario;
        DataCriacao = DateTime.UtcNow;
        Ativo = true;
    }

    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório");
        if (nome.Trim().Length > 120)
            throw new ArgumentException("Nome deve ter no máximo 120 caracteres");
        Nome = nome.Trim();
    }

    public void UpdateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório");
        var normalizado = email.Trim().ToLowerInvariant();
        if (normalizado.Length > 180)
            throw new ArgumentException("Email deve ter no máximo 180 caracteres");
        if (!normalizado.Contains('@') || !normalizado.Contains('.'))
            throw new ArgumentException("Email inválido");
        Email = normalizado;
    }

    public void UpdateSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new ArgumentException("Senha é obrigatória");
        if (senha.Length < 6)
            throw new ArgumentException("Senha deve ter no mínimo 6 caracteres");
        if (senha.Length > 100)
            throw new ArgumentException("Senha deve ter no máximo 100 caracteres");
        Senha = senha;
    }

    public void UpdateTelefone(string? telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
        {
            Telefone = null;
            return;
        }
        if (telefone.Trim().Length > 20)
            throw new ArgumentException("Telefone deve ter no máximo 20 caracteres");
        Telefone = telefone.Trim();
    }

    public void AlterarTipo(TipoUsuario tipoUsuario) => TipoUsuario = tipoUsuario;

    public void RegistrarAcesso() => UltimoAcessoEm = DateTime.UtcNow;

    public void Ativar() => Ativo = true;

    public void Desativar() => Ativo = false;

    protected Usuario() { } // EF
}
