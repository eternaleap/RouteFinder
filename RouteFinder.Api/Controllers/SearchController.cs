using Microsoft.AspNetCore.Mvc;
using RouteFinder.Domain;

namespace RouteFinder.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController(ILogger<SearchController> logger, ISearchService searchService)
    : ControllerBase
{
    [HttpPost(Name = "search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SearchResponse>> Search(SearchRequest request)
    {
        logger.LogInformation("Search request started");
        var result = await searchService.SearchAsync(request, default);

        if (result.Routes == null || result.Routes.Length == 0)
            return NotFound("No records found for such query");
        
        return Ok(result);
    }
}
