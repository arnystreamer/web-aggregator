using JetBrains.Annotations;

namespace Jimx.WebAggregator.API.Models;

[PublicAPI]
public class SortingFunctionApi(int id, string functionName, string name, string? description)
{
    public int Id { get; } = id;
    public string FunctionName { get; } = functionName;
    public string Name { get; } = name;
    public string? Description { get; } = description;
}