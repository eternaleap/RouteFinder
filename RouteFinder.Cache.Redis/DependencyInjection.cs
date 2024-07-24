using Microsoft.Extensions.DependencyInjection;
using RouteFinder.Domain;
using RouteFinder.Domain.Entities;

namespace RouteFinder.Cache.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddRouteCache(this IServiceCollection services)
    {
        return services
            .AddSingleton<ICacheRepository<SearchQuery, Route[]>, RoutesCacheRepository>();
    }
}