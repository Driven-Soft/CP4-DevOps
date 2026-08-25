using Argos.Application.Interfaces.Repositories;
using Argos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Argos.Infrastructure.Repositories;

/// <summary>
/// Implementação genérica do <see cref="IRepository{T}"/> (chave <see cref="int"/>),
/// Leituras simples usam <c>AsNoTracking</c>;
/// <see cref="GetById"/> rastreia a entidade para que o serviço a mute e salve.
/// </summary>
public class Repository<T>(ArgosContext context) : IRepository<T> where T : class
{
    protected ArgosContext Context => context;
    protected DbSet<T> Set => context.Set<T>();

    public IReadOnlyCollection<T> GetAll() => Set.AsNoTracking().ToList();
    public T? GetById(int id) => Set.Find(id);
    public void Add(T entity) => Set.Add(entity);
    public void Update(T entity) => Set.Update(entity);
    public void Delete(T entity) => Set.Remove(entity);
    public void SaveChanges() => context.SaveChanges();
}
