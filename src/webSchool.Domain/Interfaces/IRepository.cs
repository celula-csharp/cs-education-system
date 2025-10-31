namespace domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> All();
    Task<T?> ById(int id);
    Task<T> Create(T entity);
    Task<bool> Update(T entity);
    Task<bool> Delete(int id);
    public Task<bool> Save();
}
