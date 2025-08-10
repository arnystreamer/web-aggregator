namespace Jimx.WebAggregator.API.Models.Common;

public abstract class NameValue(string name, decimal value)
{
    public string Name { get; set; } = name;
    public decimal Value { get; set; } = value;
}