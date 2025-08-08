namespace Jimx.WebAggregator.Parser.Helpers;

public static class ParserHelper
{
    public static decimal? ParseToDecimal(this string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        
        return decimal.TryParse(value, out var result) ? result : null;
    }
}