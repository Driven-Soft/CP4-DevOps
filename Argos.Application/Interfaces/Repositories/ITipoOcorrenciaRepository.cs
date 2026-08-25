using Argos.Domain.Entities;

namespace Argos.Application.Interfaces.Repositories;

public interface ITipoOcorrenciaRepository : IRepository<TipoOcorrencia>
{
    /// <summary>Busca pelo slug estável usado pelo app (ex.: "alagamento") para validar unicidade.</summary>
    TipoOcorrencia? GetByChave(string chave);

    /// <summary>Lista apenas os tipos ativos — alimenta o dropdown/filtros do app.</summary>
    IReadOnlyCollection<TipoOcorrencia> ListarAtivos();
}
