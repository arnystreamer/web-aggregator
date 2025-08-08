namespace Jimx.WebAggregator.Calculations.Helpers;

public static class EnumerableSumHelper
{
    public static decimal SumOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
    {
        decimal sum = 0;
        foreach (var item in source)
        {
            sum += selector(item);
        }

        return sum;
    }
}