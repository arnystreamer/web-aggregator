namespace Jimx.WebAggregator.DownloadDataApp.Options;

public class ParsingCollateralCookie : ParsingCookie
{
    public required string Domain { get; set; }
    public required string Path { get; set; } 
}