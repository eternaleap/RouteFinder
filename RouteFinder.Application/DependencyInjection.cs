using Microsoft.Extensions.DependencyInjection;
using RouteFinder.Application.Providers;
using RouteFinder.Domain;

namespace RouteFinder.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddSearchProviders(this IServiceCollection services)
    {
        return services
            .AddSingleton<ISearchService, SearchService>()
            .AddSingleton<IProviderSearchService, ProviderOneSearchService>()
            .AddSingleton<IProviderSearchService, ProviderTwoSearchService>();
    }
}