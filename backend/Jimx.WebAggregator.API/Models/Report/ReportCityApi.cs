namespace Jimx.WebAggregator.API.Models.Report;

public class ReportCityApi
{
    public string Name { get; }
    public string Region { get; }
    public string Country { get; }
    public string? CountryCode { get; }
    public int Year { get; }
    public int Month { get; }

    public ReportCityApi(string name, string region, string country, string? countryCode, int year, int month)
    {
        Name = name;
        Region = region;
        Country = country;
        CountryCode = countryCode;
        Year = year;
        Month = month;
    }
}