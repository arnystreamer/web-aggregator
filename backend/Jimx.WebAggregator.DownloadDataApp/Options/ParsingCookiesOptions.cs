namespace Jimx.WebAggregator.DownloadDataApp.Options;

public class ParsingCookiesOptions
{
    public required string CommonDomain { get; set; } 
    public required string CommonPath { get; set; }
    public required float CommonExpirationInMs { get; set; }
    
    public required NameValue[] Cookies { get; set; }
    public required ParsingCollateralCookie[]? CollateralCookies { get; set; }
    
    public required NameValue[]? Headers { get; set; }
}