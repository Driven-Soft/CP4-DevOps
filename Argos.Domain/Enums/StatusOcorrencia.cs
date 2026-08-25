namespace Argos.Domain.Enums;

/// <summary>
/// Estados do ciclo de vida de uma ocorrência reportada.
/// Persistido como string (UPPER_SNAKE) e exposto assim no contrato REST.
/// </summary>
public enum StatusOcorrencia
{
    EM_ANALISE,
    EQUIPE_A_CAMINHO,
    EQUIPE_NO_LOCAL,
    RESOLVIDA
}
