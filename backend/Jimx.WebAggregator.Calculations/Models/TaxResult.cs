namespace Jimx.WebAggregator.Calculations.Models;

public class TaxResult
{
    public string Name { get; set; }
    public decimal Value { get; set; }

    public TaxResult(string name, decimal value)
    {
        Name = name;
        Value = value;
    }
}