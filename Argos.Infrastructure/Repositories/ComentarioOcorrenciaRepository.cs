using Argos.Application.Interfaces.Repositories;
using Argos.Domain.Entities;
using Argos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Argos.Infrastructure.Repositories;

public class ComentarioOcorrenciaRepository(ArgosContext context)
    : Repository<ComentarioOcorrencia>(context), IComentarioOcorrenciaRepository
{
    public IReadOnlyCollection<ComentarioOcorrencia> ListarPorOcorrencia(int ocorrenciaId) =>
        Set.AsNoTracking()
            .Include(c => c.Usuario)
            .Where(c => c.OcorrenciaId == ocorrenciaId && c.Ativo)
            .OrderBy(c => c.DataCriacao)
            .ToList();

    public ComentarioOcorrencia? GetByIdComUsuario(int id) =>
        Set.AsNoTracking()
            .Include(c => c.Usuario)
            .FirstOrDefault(c => c.Id == id);
}
