namespace Starfish.Shared;

public interface IRepository<T> where T: class 
{
    public Task<List<T>> GetAll(CancellationToken ctx);
    
    public Task<T?> Get(Guid id, CancellationToken ctx);
    
    public Task Add(T item, CancellationToken ctx);
    
    public Task Add(IEnumerable<T> items, CancellationToken ctx);
    
    public Task Delete(Guid id, CancellationToken ctx);
    
    public Task Delete(List<Guid> ids, CancellationToken ctx);
}