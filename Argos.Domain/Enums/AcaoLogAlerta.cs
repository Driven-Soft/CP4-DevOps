namespace Argos.Domain.Enums;

/// <summary>
/// Ação registrada na auditoria de alertas (<see cref="Entities.LogAlerta"/>).
/// Persistido como string (UPPER_SNAKE) e exposto assim no contrato REST.
/// </summary>
public enum AcaoLogAlerta
{
    CRIADO,
    EDITADO,
    ATIVADO,
    DESATIVADO,
    ENCERRADO
}
