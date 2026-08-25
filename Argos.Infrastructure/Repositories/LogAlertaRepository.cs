using Argos.Application.Interfaces.Repositories;
using Argos.Domain.Entities;
using Argos.Infrastructure.Persistence;

namespace Argos.Infrastructure.Repositories;

public class LogAlertaRepository(ArgosContext context)
    : Repository<LogAlerta>(context), ILogAlertaRepository
{
}
