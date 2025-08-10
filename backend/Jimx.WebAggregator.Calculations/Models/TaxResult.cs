namespace Jimx.WebAggregator.Calculations.Models;

public class TaxResult(string name, decimal value)
{
    public string Name { get; } = name;
    public decimal Value { get; } = value;
}