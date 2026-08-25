using Argos.Application.Interfaces.Repositories;
using Argos.Domain.Entities;
using Argos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Argos.Infrastructure.Repositories;

public class OcorrenciaRepository(ArgosContext context)
    : Repository<Ocorrencia>(context), IOcorrenciaRepository
{
    public IReadOnlyCollection<Ocorrencia> Search(string? tipoChave, string? termo)
    {
        var query = Set.AsNoTracking().Include(o => o.TipoOcorrencia).AsQueryable();

        if (!string.IsNullOrWhiteSpace(tipoChave))
            query = query.Where(o => o.TipoOcorrencia.Chave == tipoChave);
        if (!string.IsNullOrWhiteSpace(termo))
            query = query.Where(o => o.Titulo.Contains(termo) || o.Descricao.Contains(termo));

        return query.OrderByDescending(o => o.DataCriacao).ToList();
    }

    public Ocorrencia? GetByIdComTipo(int id) =>
        Set.AsNoTracking()
            .Include(o => o.TipoOcorrencia)
            .FirstOrDefault(o => o.Id == id);
}
