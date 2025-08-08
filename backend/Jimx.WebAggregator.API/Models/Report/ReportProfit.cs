namespace Jimx.WebAggregator.API.Models.Report;

public class ReportProfit
{
    public MultiCurrencyValue ValueGross { get; }
    
    public MultiCurrencyValue ValueNet { get; }

    public ReportProfit(decimal valueGross, decimal valueNet, decimal crossRateToUsd)
    {
        ValueGross = new MultiCurrencyValue(valueGross, valueGross * crossRateToUsd);
        ValueNet = new MultiCurrencyValue(valueNet, valueNet * crossRateToUsd);
    }
}