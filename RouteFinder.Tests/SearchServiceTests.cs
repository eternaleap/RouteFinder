using Microsoft.Extensions.Logging;
using Moq;
using RouteFinder.Domain;
using RouteFinder.Domain.Entities;

namespace RouteFinder.Tests;

public class SearchServiceTests
{
    [Fact]
    public async Task SearchAsync_WithAllProvidersUnavailable_ReturnsNoRoutes()
    {
        // Arrange
        var firstProvider = new Mock<IProviderSearchService>();
        firstProvider.Setup(p => p.IsAvailable()).ReturnsAsync(false);
        var secondProvider = new Mock<IProviderSearchService>();
        secondProvider.Setup(p => p.IsAvailable()).ReturnsAsync(false);
        
        var mockSearchProviders = new List<IProviderSearchService>
        {
            firstProvider.Object,
            secondProvider.Object
        };
        var mockCacheProviderSearchService = new Mock<ICacheProviderSearchService>();
        var loggerMock = new Mock<ILogger<SearchService>>();
        var searchService = new SearchService(mockSearchProviders, mockCacheProviderSearchService.Object, loggerMock.Object);
        var query = new SearchQuery();

        // Act
        var result = await searchService.SearchAsync(query, default);

        // Assert
        Assert.Empty(result.Routes);
    }

    [Fact]
    public async Task SearchAsync_WithCacheEntriesNotMatchedOneServiceAvailable_ReturnsRoutesFromProvider()
    {
        // Arrange
        var mockProviderOne = new Mock<IProviderSearchService>();
        var mockProviderTwo = new Mock<IProviderSearchService>();

        mockProviderOne.Setup(p => p.Search(It.IsAny<SearchQuery>()))
            .ReturnsAsync(new[]
            {
                new Route { Id = Guid.NewGuid(), Origin = "Moscow", Destination = "Tula", Price = 250, DestinationDateTime = DateTime.Now.AddHours(5) }
            });
        mockProviderTwo.Setup(p => p.IsAvailable()).ReturnsAsync(false);
        
        mockProviderTwo.Setup(p => p.Search(It.IsAny<SearchQuery>()))
            .ReturnsAsync(new[]
            {
                new Route { Id = Guid.NewGuid(), Origin = "Seattle", Destination = "Denver", Price = 150, OriginDateTime = DateTime.Now, DestinationDateTime = DateTime.Now.AddHours(4) },
                new Route { Id = Guid.NewGuid(), Origin = "Houston", Destination = "Dallas", Price = 250, OriginDateTime = DateTime.Now,  DestinationDateTime = DateTime.Now.AddHours(5) },
                new Route { Id = Guid.NewGuid(), Origin = "Moscow", Destination = "Kaliningrad", Price = 250, OriginDateTime = DateTime.Now,  DestinationDateTime = DateTime.Now.AddHours(5) }
            });
        mockProviderTwo.Setup(p => p.IsAvailable()).ReturnsAsync(true);

        var mockSearchProviders = new List<IProviderSearchService> { mockProviderOne.Object, mockProviderTwo.Object };
        var mockCacheProviderSearchService = new Mock<ICacheProviderSearchService>();
        var cachedRoutes = new Route[] { };
        mockCacheProviderSearchService.Setup(c => c.GetAsyns(It.IsAny<SearchQuery>()))
            .ReturnsAsync(cachedRoutes);
        var loggerMock = new Mock<ILogger<SearchService>>();
        var searchService = new SearchService(mockSearchProviders, mockCacheProviderSearchService.Object, loggerMock.Object);
        var query = new SearchQuery
        {
            Filters = new SearchFiltersQuery()
            {
                OnlyCached = false
            }
        };

        // Act
        var result = await searchService.SearchAsync(query, default);

        // Assert
        Assert.NotEmpty(result.Routes);
        Assert.Equal(3, result.Routes.Length);
        Assert.Contains(result.Routes, r => r.Origin == "Seattle");
        Assert.Contains(result.Routes, r => r.Destination != "Moscow");
        Assert.Equal(250, result.MaxPrice);
        Assert.Equal(150, result.MinPrice);
        Assert.Equal(300, result.MaxMinutesRoute);
        Assert.Equal(240, result.MinMinutesRoute);
        Assert.Contains(result.Routes, r => r.Destination == "Kaliningrad");
    }
    
        [Fact]
    public async Task SearchAsync_WithCacheEntriesMatched_ReturnsRoutesFromCache()
    {
        // Arrange
        var mockProviderOne = new Mock<IProviderSearchService>();
        var mockProviderTwo = new Mock<IProviderSearchService>();

        mockProviderOne.Setup(p => p.Search(It.IsAny<SearchQuery>()))
            .ReturnsAsync(new[]
            {
                new Route { Id = Guid.NewGuid(), Origin = "Moscow", Destination = "Tula", Price = 250, DestinationDateTime = DateTime.Now.AddHours(5) }
            });
        mockProviderTwo.Setup(p => p.IsAvailable()).ReturnsAsync(false);
        
        mockProviderTwo.Setup(p => p.Search(It.IsAny<SearchQuery>()))
            .ReturnsAsync(new[]
            {
                new Route { Id = Guid.NewGuid(), Origin = "Seattle", Destination = "Denver", Price = 150, DestinationDateTime = DateTime.Now.AddHours(4) },
            });
        mockProviderTwo.Setup(p => p.IsAvailable()).ReturnsAsync(true);

        var mockSearchProviders = new List<IProviderSearchService> { mockProviderOne.Object, mockProviderTwo.Object };
        var mockCacheProviderSearchService = new Mock<ICacheProviderSearchService>();
        var cachedRoutes = new Route[]
        {
            new() { Id = Guid.NewGuid(), Origin = "Moscow", Destination = "Tula", Price = 250, DestinationDateTime = DateTime.Now.AddHours(5) },
            new() { Id = Guid.NewGuid(), Origin = "Seattle", Destination = "Denver", Price = 50, DestinationDateTime = DateTime.Now.AddHours(4) },
            new() { Id = Guid.NewGuid(), Origin = "Some Point", Destination = "Destination", Price = 150, DestinationDateTime = DateTime.Now.AddHours(4) },
        };
        mockCacheProviderSearchService.Setup(c => c.GetAsyns(It.IsAny<SearchQuery>()))
            .ReturnsAsync(cachedRoutes);
        var loggerMock = new Mock<ILogger<SearchService>>();
        var searchService = new SearchService(mockSearchProviders, mockCacheProviderSearchService.Object, loggerMock.Object);
        var query = new SearchQuery
        {
            Filters = new SearchFiltersQuery()
            {
                OnlyCached = false
            }
        };

        // Act
        var result = await searchService.SearchAsync(query, default);

        // Assert
        Assert.NotEmpty(result.Routes);
        Assert.Equal(3, result.Routes.Length);
        Assert.Equal(250, result.MaxPrice);
        Assert.Equal(50, result.MinPrice);
        Assert.Equal(cachedRoutes.Max(r => (int)(r.DestinationDateTime - r.OriginDateTime).TotalMinutes), result.MaxMinutesRoute);
        Assert.Equal(cachedRoutes.Min(r => (int)(r.DestinationDateTime - r.OriginDateTime).TotalMinutes), result.MinMinutesRoute);
        Assert.Contains(result.Routes, r => r.Destination == "Destination");
    }
}