using Argos.Domain.Entities;

namespace Argos.Application.Interfaces.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    /// <summary>Busca por email (normalizado em lower-case) para validar unicidade no cadastro.</summary>
    Usuario? GetByEmail(string email);
}
