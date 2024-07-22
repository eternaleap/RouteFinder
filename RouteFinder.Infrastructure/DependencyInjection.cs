using Microsoft.Extensions.DependencyInjection;
using RouteFinder.Application.Gateways;
using RouteFinder.Domain;
using RouteFinder.Infrastructure.Gateways;

namespace RouteFinder.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSearchProvidersImplementations(this IServiceCollection services)
    {
        return services
            .AddSingleton<ICacheProviderSearchService, CacheProviderSearchService>()
            .AddSingleton<IProviderOneSearchGateway, ProviderOneSearchGateway>()
            .AddSingleton<IProviderTwoSearchGateway, ProviderTwoSearchGateway>();
    }
}