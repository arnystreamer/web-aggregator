using Jimx.WebAggregator.API.Models.CityCosts;
using Jimx.WebAggregator.API.Models.Report;

namespace Jimx.WebAggregator.API.Services;

public class SalaryTypesService
{
    private readonly SalaryTypeApi[] _salaryTypes =
    [
        new(1, "Zero", "Zero value"),
        new(2, "Expat salary", "Expat salary based on Source 1. Can be multiplied"),
        new(3, "Manual", "Entered value"),
        new(4, "Developer bottom 25%", "Developers P25 salary based on aggregated Source 2. Can be multiplied"),
        new(5, "Developer top 25%", "Developers P75 salary based on aggregated Source 2. Can be multiplied")
    ];
    
    public SalaryTypeApi[] GetAll()
    {
        return _salaryTypes;
    }

    public SalaryTypeApi Get(int id)
    {
        return _salaryTypes.First(s => s.Id == id);
    }
}