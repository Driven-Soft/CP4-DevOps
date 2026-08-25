using Argos.Application.Interfaces.Repositories;
using Argos.Domain.Entities;
using Argos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Argos.Infrastructure.Repositories;

public class UsuarioRepository(ArgosContext context)
    : Repository<Usuario>(context), IUsuarioRepository
{
    public Usuario? GetByEmail(string email) =>
        Set.AsNoTracking().FirstOrDefault(u => u.Email == email);
}
