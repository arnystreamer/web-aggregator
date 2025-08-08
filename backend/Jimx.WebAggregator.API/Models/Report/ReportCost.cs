namespace Jimx.WebAggregator.API.Models.Report;

public class ReportCost
{
    public MultiCurrencyValue ValueNet { get; }

    public ReportCost(decimal valueNet, decimal crossRateToUsd)
    {
        ValueNet = new MultiCurrencyValue(valueNet, valueNet * crossRateToUsd);
    }
}