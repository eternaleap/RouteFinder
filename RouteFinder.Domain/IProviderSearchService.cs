namespace RouteFinder.Domain;

public interface IProviderSearchService
{
    public Task<IReadOnlyCollection<Route>> Search(SearchRequest request);

    public Task<bool> IsAvailable();
}
