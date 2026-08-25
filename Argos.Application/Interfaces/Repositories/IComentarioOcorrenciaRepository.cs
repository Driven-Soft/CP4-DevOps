using Argos.Domain.Entities;

namespace Argos.Application.Interfaces.Repositories;

public interface IComentarioOcorrenciaRepository : IRepository<ComentarioOcorrencia>
{
    /// <summary>
    /// Lista os comentários ativos de uma ocorrência com <see cref="ComentarioOcorrencia.Usuario"/>
    /// carregado, para o DTO derivar <c>autor</c> e <c>papel</c>.
    /// </summary>
    IReadOnlyCollection<ComentarioOcorrencia> ListarPorOcorrencia(int ocorrenciaId);

    /// <summary>Comentário único com o autor carregado (resposta do POST de comentário).</summary>
    ComentarioOcorrencia? GetByIdComUsuario(int id);
}
