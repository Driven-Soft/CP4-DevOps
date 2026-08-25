using System.ComponentModel.DataAnnotations;
using Argos.Domain.Entities;
using Argos.Domain.Enums;

namespace Argos.Application.DTOs;

/// <summary>Corpo do <c>POST /usuarios</c> (cadastro local — retorna o <c>id</c>).</summary>
public class UsuarioRequest
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = null!;

    [Required, EmailAddress, MaxLength(180)]
    public string Email { get; set; } = null!;

    [Required, MinLength(6), MaxLength(100)]
    public string Senha { get; set; } = null!;

    [MaxLength(20)]
    public string? Telefone { get; set; }

    public TipoUsuario TipoUsuario { get; set; } = TipoUsuario.CIDADAO;

    public Usuario ToDomain() => new(Nome, Email, Senha, Telefone, TipoUsuario);
}

/// <summary>PATCH parcial; o serviço aplica cada campo só quando enviado.</summary>
public class UsuarioPatchRequest
{
    [MaxLength(120)]
    public string? Nome { get; set; }

    [MinLength(6), MaxLength(100)]
    public string? Senha { get; set; }

    [MaxLength(20)]
    public string? Telefone { get; set; }

    public TipoUsuario? TipoUsuario { get; set; }

    public bool? Ativo { get; set; }
}

/// <summary>Modelo de leitura — nunca expõe a <c>Senha</c>.</summary>
public class UsuarioResponse
{
    public int Id { get; }
    public string Nome { get; }
    public string Email { get; }
    public string? Telefone { get; }
    public TipoUsuario TipoUsuario { get; }
    public DateTime DataCriacao { get; }

    public UsuarioResponse(Usuario usuario)
    {
        Id = usuario.Id;
        Nome = usuario.Nome;
        Email = usuario.Email;
        Telefone = usuario.Telefone;
        TipoUsuario = usuario.TipoUsuario;
        DataCriacao = usuario.DataCriacao;
    }
}
