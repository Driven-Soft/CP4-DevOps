namespace Argos.Domain.Enums;

/// <summary>
/// Perfil do usuário do app. Define o papel exibido em comentários
/// (cidadão vs. defesa civil) e o nível de permissão de ações oficiais.
/// </summary>
public enum TipoUsuario
{
    CIDADAO,
    DEFESA_CIVIL,
    ADMIN
}
