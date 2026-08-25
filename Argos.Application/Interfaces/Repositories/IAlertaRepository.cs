using Argos.Domain.Entities;
using Argos.Domain.Enums;

namespace Argos.Application.Interfaces.Repositories;

public interface IAlertaRepository : IRepository<Alerta>
{
    /// <summary>
    /// Feed de alertas (<c>?status=&amp;nivel=</c>) com as navegações
    /// <see cref="Alerta.ZonaRisco"/> e <see cref="Alerta.UsuarioCriador"/> carregadas
    /// para o DTO composto.
    /// </summary>
    IReadOnlyCollection<Alerta> Buscar(bool? apenasAtivos, NivelRisco? nivel);

    /// <summary>Detalhe do alerta com ZonaRisco + UsuarioCriador carregados (DTO composto).</summary>
    Alerta? GetByIdCompleto(int id);
}
