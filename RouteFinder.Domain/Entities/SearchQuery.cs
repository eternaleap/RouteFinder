namespace RouteFinder.Domain.Entities;

public class SearchQuery
{
    // Mandatory
    // Start point of route, e.g. Moscow 
    public string Origin { get; set; }
    
    // Mandatory
    // End point of route, e.g. Sochi
    public string Destination { get; set; }
    
    // Mandatory
    // Start date of route
    public DateTime OriginDateTime { get; set; }
    
    // Optional
    public SearchFiltersQuery? Filters { get; set; }
    
    public override int GetHashCode() =>
        HashCode.Combine(Origin, Destination, OriginDateTime, Filters);

    public override bool Equals(object? obj)
    {
        return obj?.GetHashCode() == GetHashCode();
    }
}

public class SearchFiltersQuery
{
    // Optional
    // End date of route
    public DateTime? DestinationDateTime { get; set; }
    
    // Optional
    // Maximum price of route
    public decimal? MaxPrice { get; set; }
    
    // Optional
    // Minimum value of timelimit for route
    public DateTime? MinTimeLimit { get; set; }
    
    // Optional
    // Forcibly search in cached data
    public bool? OnlyCached { get; set; }

    public override int GetHashCode() => HashCode.Combine(DestinationDateTime, MaxPrice, MinTimeLimit);
}