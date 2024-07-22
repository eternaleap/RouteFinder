using Microsoft.AspNetCore.Mvc;
using RouteFinder.Domain;

namespace RouteFinder.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController(ILogger<SearchController> logger, ISearchService searchService)
    : ControllerBase
{
    [HttpPost(Name = "search")]
    public async Task<SearchResponse> Search(SearchRequest request)
    {
        logger.LogInformation("Request started");
        return await searchService.SearchAsync(request, default);
    }
}
