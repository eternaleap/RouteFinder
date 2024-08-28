using RouteFinder.Domain;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RouteFinder.Domain.Entities;

namespace RouteFinder.DragonFly;

public class RoutesCacheRepository : ICacheRepository<SearchQuery, Route[]>, IDisposable
{
    private readonly int _cacheTtl;
    private bool _isDisposed;
    private readonly ILogger<RoutesCacheRepository> _logger;
    private readonly MemcachedClient _client;
    
    public RoutesCacheRepository(ILogger<RoutesCacheRepository> logger)
    {
        _logger = logger;
        _cacheTtl = 300000;
        
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Warning));
        var config = new MemcachedClientConfiguration(loggerFactory, new MemcachedClientOptions());
        _client = new MemcachedClient(loggerFactory, config); 
    }
    
    public async Task DeleteAsync(SearchQuery key)
    {
        bool success = await _client.RemoveAsync(key.GetHashCode().ToString());
        
        if (success) {
            _logger.LogInformation($"Key {key.GetHashCode().ToString()} deleted");
        } else {
            _logger.LogError($"Failed to remove key {key.GetHashCode().ToString()}.");
        }
    }

    public async Task<bool> ExistsAsync(SearchQuery key)
    {
        var result = await _client.GetAsync(key.GetHashCode().ToString());
        return result.Success;
    }
        

    public async Task<Route[]> GetAsync(SearchQuery key)
    {
        var data = await _client.GetAsync(key.GetHashCode().ToString());

        _logger.LogInformation(data.HasValue
            ? $"Found records for the key {key.GetHashCode()}"
            : $"No records found for the key {key.GetHashCode()}");

        return data.HasValue ? JsonConvert.DeserializeObject<Route[]>(data.Value.ToString()) : null;
    }

    public async Task<bool> SetAsync(SearchQuery key, Route[] value, TimeSpan? expiry = null) =>
        await _client.SetAsync(
            key.GetHashCode().ToString(), 
            value,
            expiry.HasValue ? expiry.Value : new TimeSpan(_cacheTtl));
 
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) 
            return;
        
        if (disposing) {
            _client.Dispose();
        }
 
        _isDisposed = true;
    }
 
    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
 
    ~RoutesCacheRepository() {
        Dispose(disposing: false);
    }
}