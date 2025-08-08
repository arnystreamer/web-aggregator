namespace Jimx.WebAggregator.API.Models.Report;

public class ReportCityApi
{
    public string Name { get; }
    public string Region { get; }
    public string Country { get; }
    public string? CountryCode { get; }

    public ReportCityApi(string name, string region, string country, string? countryCode)
    {
        Name = name;
        Region = region;
        Country = country;
        CountryCode = countryCode;
    }
}