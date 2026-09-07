namespace Jimx.WebAggregator.Parser.Helpers;

public static class ParserHelper
{
    public static decimal? ParseToDecimal(this string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        var finalValue = value.Trim();
        if (finalValue.Length == 0 || finalValue == "?")
        {
            return null;
        }

        return decimal.TryParse(finalValue, out var result) ? result : null;
    }

    public static decimal? ParseToDecimalRangeDashedLeft(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        var middleIndex = value.IndexOf('-');
        if (middleIndex < 0)
        {
            return null;
        }
        
        return ParseToDecimal(value[..middleIndex]);
    }
    
    public static decimal? ParseToDecimalRangeDashedRight(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        var middleIndex = value.IndexOf('-');
        if (middleIndex < 0 || middleIndex >= value.Length - 1)
        {
            return null;
        }
        
        return ParseToDecimal(value[(middleIndex+1)..]);
    }
}