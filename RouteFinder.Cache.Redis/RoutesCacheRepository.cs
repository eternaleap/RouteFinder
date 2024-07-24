using Microsoft.Extensions.Logging;
using RouteFinder.Domain;
using RouteFinder.Domain.Entities;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace RouteFinder.Cache.Redis;

public class RoutesCacheRepository : ICacheRepository<SearchQuery, Route[]>, IDisposable
{
    private readonly ConnectionMultiplexer _redis; 
    private readonly IDatabase _database;
    private bool _disposedValue;
    private readonly ILogger<RoutesCacheRepository> _logger;
    
    public RoutesCacheRepository(ILogger<RoutesCacheRepository> logger)
    {
        _logger = logger;
        _redis = ConnectionMultiplexer.Connect("cache-redis:6379");
        _database = _redis.GetDatabase();
    }
    
    public Task DeleteAsync(SearchQuery key)
    {
        var _ = _database.StringGetDeleteAsync(key.GetHashCode().ToString()).ConfigureAwait(false);
        return Task.CompletedTask;
    }
 
    public async Task<bool> ExistsAsync(SearchQuery key) =>
        await _database.KeyExistsAsync(key.GetHashCode().ToString()).ConfigureAwait(false);

    public async Task<Route[]> GetAsync(SearchQuery key)
    {
        var data = await _database.StringGetAsync(key.GetHashCode().ToString()).ConfigureAwait(false);
        
        if(data.HasValue)
            _logger.LogInformation($"Found {data.Length()} records for the key {key.GetHashCode()}");
        else
            _logger.LogInformation($"No records found for the key {key.GetHashCode()}");
        
        return data.HasValue ? JsonConvert.DeserializeObject<Route[]>(data.ToString()) : null;
    }

    public async Task<bool> SetAsync(SearchQuery key, Route[] value, TimeSpan? expiry = null) =>
        await _database.StringSetAsync(key.GetHashCode().ToString(), JsonConvert.SerializeObject(value), expiry)
            .ConfigureAwait(false);
 
    protected virtual void Dispose(bool disposing) {
        if (!_disposedValue) {
            if (disposing) {
                _redis.Dispose();
            }
 
            _disposedValue = true;
        }
    }
 
    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
 
    ~RoutesCacheRepository() {
        Dispose(disposing: false);
    }
}