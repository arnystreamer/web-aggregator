using Jimx.WebAggregator.API.Models.Common;

namespace Jimx.WebAggregator.API.Models.Report;

public class CostBit(string name, decimal value) : NameValue(name, value);