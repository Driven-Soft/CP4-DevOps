using Argos.Application.Interfaces.Repositories;
using Argos.Domain.Entities;
using Argos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Argos.Infrastructure.Repositories;

public class TipoOcorrenciaRepository(ArgosContext context)
    : Repository<TipoOcorrencia>(context), ITipoOcorrenciaRepository
{
    public TipoOcorrencia? GetByChave(string chave) =>
        Set.AsNoTracking().FirstOrDefault(t => t.Chave == chave);

    public IReadOnlyCollection<TipoOcorrencia> ListarAtivos() =>
        Set.AsNoTracking().Where(t => t.Ativo).OrderBy(t => t.Nome).ToList();
}
