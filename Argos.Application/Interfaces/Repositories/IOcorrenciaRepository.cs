using Argos.Domain.Entities;

namespace Argos.Application.Interfaces.Repositories;

public interface IOcorrenciaRepository : IRepository<Ocorrencia>
{
    /// <summary>
    /// Feed/busca de ocorrências (<c>?tipo=&amp;q=</c>) com <see cref="Ocorrencia.TipoOcorrencia"/>
    /// carregado (o DTO expõe <c>tipo = Chave</c>), ordenado por data decrescente.
    /// </summary>
    IReadOnlyCollection<Ocorrencia> Search(string? tipoChave, string? termo);

    /// <summary>Detalhe/registro com <see cref="Ocorrencia.TipoOcorrencia"/> carregado para o DTO.</summary>
    Ocorrencia? GetByIdComTipo(int id);
}
