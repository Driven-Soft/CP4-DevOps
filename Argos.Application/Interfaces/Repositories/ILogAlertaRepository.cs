using Argos.Domain.Entities;

namespace Argos.Application.Interfaces.Repositories;

/// <summary>
/// Auditoria (opcional) de alertas. Escrito pelo <c>AlertaService</c> nas ações de
/// criação/edição/ativação.
/// </summary>
public interface ILogAlertaRepository : IRepository<LogAlerta>
{
}
