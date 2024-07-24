namespace RouteFinder.Domain;

public interface ICacheRepository<TKey, TValue>
{
    Task DeleteAsync(TKey key);
    Task<bool> ExistsAsync(TKey key);
    Task<TValue>GetAsync(TKey key);
    Task <bool> SetAsync(TKey key, TValue value, TimeSpan ? expiry = null);
}