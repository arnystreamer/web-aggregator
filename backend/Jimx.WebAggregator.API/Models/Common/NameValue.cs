namespace Jimx.WebAggregator.API.Models.Common;

public abstract class NameValue
{
    public string Name { get; set; }
    public decimal Value { get; set; }

    public NameValue(string name, decimal value)
    {
        Name = name;
        Value = value;
    }
}