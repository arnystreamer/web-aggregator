using Jimx.WebAggregator.API.Models.Common;

namespace Jimx.WebAggregator.API.Models.Report;

public class CostBit : NameValue
{
    public CostBit(string name, decimal value) : base(name, value)
    {
    }
}