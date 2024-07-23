using RouteFinder.Domain.Entities;

namespace RouteFinder.Domain;

public interface IProviderSearchService
{
    public Task<IReadOnlyCollection<Route>> Search(SearchQuery query);

    public Task<bool> IsAvailable();
}
