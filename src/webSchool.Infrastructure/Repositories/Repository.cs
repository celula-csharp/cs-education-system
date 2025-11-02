using domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using infrastructure.Data;

namespace infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _db;
    private readonly DbSet<T> _set;

    public Repository(AppDbContext db)
    {
        _db = db;
        _set = _db.Set<T>();
    }

    public async Task<List<T>> All() => await _set.ToListAsync();
    public async Task<T?> ById(int id) => await _set.FindAsync(id);
    public async Task<T> Create(T entity)
    {
        _set.Add(entity);
        return await Task.FromResult(entity);
    }
    public async Task<bool> Update(T entity)
    {
        _set.Update(entity);
        return await Task.FromResult(true);
    }
    public async Task<bool> Delete(int id)
    {
        var entity = await _set.FindAsync(id);
        if (entity == null) return false;
        _set.Remove(entity);
        return true;
    }
    public async Task<bool> Save()
    {
        await _db.SaveChangesAsync();
        return true;
    }
}
