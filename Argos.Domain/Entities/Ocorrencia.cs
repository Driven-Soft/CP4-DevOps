using Argos.Domain.Enums;

namespace Argos.Domain.Entities;

/// <summary>
/// Incidente reportado por um usuário. Alimenta feed, detalhe, registro e
/// pinos no mapa. A distância não é persistida — é calculada no cliente.
/// </summary>
public class Ocorrencia
{
    public int Id { get; private set; }
    public string Titulo { get; private set; } = null!;
    public string Descricao { get; private set; } = null!;
    public int TipoOcorrenciaId { get; private set; }
    public TipoOcorrencia TipoOcorrencia { get; private set; } = null!;
    public NivelRisco NivelRisco { get; private set; }
    public StatusOcorrencia Status { get; private set; }
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
    public int? ZonaRiscoId { get; private set; }
    public ZonaRisco? ZonaRisco { get; private set; }
    public string? Bairro { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }
    public DateTime? ResolvidoEm { get; private set; }
    public List<ComentarioOcorrencia> Comentarios { get; private set; } = [];

    public Ocorrencia(string titulo, string descricao, int tipoOcorrenciaId,
        int usuarioId, double latitude, double longitude,
        NivelRisco nivelRisco = NivelRisco.MEDIO,
        int? zonaRiscoId = null, string? bairro = null)
    {
        UpdateTitulo(titulo);
        UpdateDescricao(descricao);
        UpdateCoordenadas(latitude, longitude);
        UpdateBairro(bairro);
        TipoOcorrenciaId = tipoOcorrenciaId;
        UsuarioId = usuarioId;
        NivelRisco = nivelRisco;
        ZonaRiscoId = zonaRiscoId;
        Status = StatusOcorrencia.EM_ANALISE;
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

    public void UpdateBairro(string? bairro)
    {
        if (string.IsNullOrWhiteSpace(bairro))
        {
            Bairro = null;
            return;
        }
        if (bairro.Trim().Length > 120)
            throw new ArgumentException("Bairro deve ter no máximo 120 caracteres");
        Bairro = bairro.Trim();
    }

    public void UpdateCoordenadas(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90)
            throw new ArgumentException("Latitude deve estar entre -90 e 90");
        if (longitude is < -180 or > 180)
            throw new ArgumentException("Longitude deve estar entre -180 e 180");
        Latitude = latitude;
        Longitude = longitude;
    }

    public void AlterarNivelRisco(NivelRisco nivelRisco)
    {
        NivelRisco = nivelRisco;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void VincularZona(int? zonaRiscoId)
    {
        ZonaRiscoId = zonaRiscoId;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AlterarStatus(StatusOcorrencia status)
    {
        Status = status;
        DataAtualizacao = DateTime.UtcNow;
        ResolvidoEm = status == StatusOcorrencia.RESOLVIDA ? DateTime.UtcNow : null;
    }

    protected Ocorrencia() { } // EF
}
