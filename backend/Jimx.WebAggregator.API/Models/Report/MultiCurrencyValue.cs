namespace Jimx.WebAggregator.API.Models.Report;

public class MultiCurrencyValue
{
    public decimal Value { get; }
    public decimal ValueInUsd { get; }

    public MultiCurrencyValue(decimal value, decimal valueInUsd)
    {
        Value = value;
        ValueInUsd = valueInUsd;
    }

    public MultiCurrencyValue ApplyMultiplicator(decimal multiplicator)
    {
        return new MultiCurrencyValue(Value * multiplicator, ValueInUsd * multiplicator);
    }
}