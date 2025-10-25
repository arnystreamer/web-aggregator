namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Parameters;

public record JobSearchParameters(
    JobSearchInterval Interval,
    JobSearchLocations Locations,
    string Keywords,
    JobSearchGeo Geo,
    bool? IsEarlyApplicant,
    JobSearchSorting? SortBy
    );