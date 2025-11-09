using System.Linq.Expressions;
using HotelHaloHub.API.Repositories.Interfaces;

namespace HotelHaloHub.API.Repositories.Implementations;

public class InMemoryRepository<T> : IRepository<T> where T : class
{
    protected readonly List<T> _collection;
    protected readonly Func<T, string> _idSelector;

    public InMemoryRepository(List<T> collection, Func<T, string> idSelector)
    {
        _collection = collection;
        _idSelector = idSelector;
    }

    public Task<IEnumerable<T>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<T>>(_collection.ToList());
    }

    public Task<T?> GetByIdAsync(string id)
    {
        var item = _collection.FirstOrDefault(x => _idSelector(x) == id);
        return Task.FromResult(item);
    }

    public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        var compiled = predicate.Compile();
        var results = _collection.Where(compiled).ToList();
        return Task.FromResult<IEnumerable<T>>(results);
    }

    public Task<T> CreateAsync(T entity)
    {
        _collection.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<bool> UpdateAsync(string id, T entity)
    {
        var index = _collection.FindIndex(x => _idSelector(x) == id);
        if (index == -1)
            return Task.FromResult(false);

        _collection[index] = entity;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(string id)
    {
        var item = _collection.FirstOrDefault(x => _idSelector(x) == id);
        if (item == null)
            return Task.FromResult(false);

        _collection.Remove(item);
        return Task.FromResult(true);
    }

    public Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        if (predicate == null)
            return Task.FromResult<long>(_collection.Count);

        var compiled = predicate.Compile();
        return Task.FromResult<long>(_collection.Count(compiled));
    }
}
