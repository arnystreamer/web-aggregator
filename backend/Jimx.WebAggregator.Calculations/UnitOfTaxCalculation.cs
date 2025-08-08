using Jimx.WebAggregator.Domain.CityCosts;

namespace Jimx.WebAggregator.Calculations;

public class UnitOfTaxCalculation
{
    private readonly UserTaxProfile _userTaxProfile;

    internal UnitOfTaxCalculation(UserTaxProfile userTaxProfile)
    {
        _userTaxProfile = userTaxProfile;
    }

    public UnitOfTaxCalculationWithTaxes WithTaxes(RegionTaxDeduction[] taxDeductions)
    {
        return new UnitOfTaxCalculationWithTaxes(_userTaxProfile, taxDeductions);
    }
}