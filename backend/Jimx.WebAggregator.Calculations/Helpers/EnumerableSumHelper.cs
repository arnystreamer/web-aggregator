namespace Jimx.WebAggregator.Calculations.Helpers;

public static class EnumerableSumHelper
{
    public static decimal SumOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
    {
        return source.Sum(selector);
    }
}