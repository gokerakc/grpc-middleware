namespace Starfish.Shared;

public interface IRepository<T> where T: class 
{
    public Task<List<T>> GetAllAsync(CancellationToken ctx);
    
    public Task<T?> GetAsync(Guid id, CancellationToken ctx);
    
    public Task AddAsync(T item, CancellationToken ctx);
    
    public Task AddAsync(IEnumerable<T> items, CancellationToken ctx);
    
    public Task DeleteAsync(Guid id, CancellationToken ctx);
    
    public Task DeleteAsync(List<Guid> ids, CancellationToken ctx);
}