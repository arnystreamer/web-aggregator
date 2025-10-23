namespace Jimx.WebAggregator.Browser.WebCrawler;

public record JobSearchParameters(
    JobSearchInterval Interval,
    JobSearchLocations Locations,
    string Keywords,
    JobSearchGeo Geo
    );