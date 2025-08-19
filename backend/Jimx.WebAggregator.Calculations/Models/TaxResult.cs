namespace Jimx.WebAggregator.Calculations.Models;

public class TaxResult(string? id, string name, decimal value, decimal multiplier)
{
    public string? Id { get; } = id;
    public string Name { get; } = name;
    public decimal Value { get; } = value;
    
    public decimal Multiplier { get; } = multiplier;
    
    public decimal ValueWithoutMultiplier => Value / Multiplier;
}