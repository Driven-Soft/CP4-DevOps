namespace Argos.Application.Interfaces.Repositories;

/// <summary>
/// Repositório genérico (chave <see cref="int"/>).
/// A persistência só efetiva no <see cref="SaveChanges"/> — o serviço orquestra
/// carregar → mutar o domínio → salvar.
/// </summary>
public interface IRepository<T> where T : class
{
    IReadOnlyCollection<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    void SaveChanges();
}
