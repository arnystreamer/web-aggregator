using Jimx.WebAggregator.Calculations.Helpers;
using Jimx.WebAggregator.Calculations.Models;
using Jimx.WebAggregator.Domain.CityCosts;

namespace Jimx.WebAggregator.Calculations;

public class UnitOfTaxCalculationWithTaxes
{
    private readonly UserTaxProfile _userTaxProfile;
    private readonly RegionTaxDeduction[] _taxDeductions;

    internal UnitOfTaxCalculationWithTaxes(UserTaxProfile userTaxProfile, RegionTaxDeduction[] taxDeductions)
    {
        _userTaxProfile = userTaxProfile;
        _taxDeductions = taxDeductions;
    }
    
    public AppliedTaxesResult ApplyTaxes(decimal annualSalaryGross)
    {
        var taxResults = new List<TaxResult>();

        var taxesGroupedByOrder = _taxDeductions
            .SelectMany(d => d.IncomeTaxes)
            .Where(tax => TaxFunctions.IsTaxApplicable(_userTaxProfile, tax))
            .GroupBy(tax => tax.Order ?? 0)
            .OrderBy(taxGroup => taxGroup.Key)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.ToArray());

        var currentTaxBase = annualSalaryGross;
        foreach (var taxGroup in taxesGroupedByOrder)
        {
            var innerTaxResults = new List<TaxResult>();

            var nonDependantGrossTaxes = taxGroup.Value.Where(tax => string.IsNullOrEmpty(tax.ApplyOn) || tax.ApplyOn == "gross").ToArray();
            innerTaxResults.AddRange(nonDependantGrossTaxes.Select(tax =>
                TaxFunctions.GetGrossTaxValue(tax, currentTaxBase)));

            var dependantGrossTaxes = taxGroup.Value.Where(tax => !string.IsNullOrEmpty(tax.ApplyOn) && tax.ApplyOn.StartsWith("tax")).ToArray();
            innerTaxResults.AddRange(dependantGrossTaxes.Select(tax => 
                TaxFunctions.GetTaxOnTaxValue(
                    tax, 
                    taxId => innerTaxResults.First(t => t.Id == taxId).ValueWithoutMultiplier)));

            var housingTaxes = taxGroup.Value.Where(tax => tax.ApplyOn == "housing").ToArray();
            innerTaxResults.AddRange(housingTaxes.Select(tax => TaxFunctions.GetHousingTaxValue(tax, 1)));

            var personTaxes = taxGroup.Value.Where(tax => tax.ApplyOn == "person").ToArray();
            innerTaxResults.AddRange(personTaxes.Select(tax => TaxFunctions.GetPersonTaxValue(tax, 1)));

            var ownedHousingTaxes = taxGroup.Value.Where(tax => tax.ApplyOn == "owned_housing").ToArray();
            innerTaxResults.AddRange(ownedHousingTaxes.Select(tax => TaxFunctions.GetOwnedHousingTaxValue(tax, 0)));

            taxResults.AddRange(innerTaxResults);
            var currentDeductions = innerTaxResults.SumOrDefault(r => r.Value);

            if (currentTaxBase > currentDeductions)
            {
                currentTaxBase -= currentDeductions;
            }
            else
            {
                currentTaxBase = 0m;
            }
        }

        return new AppliedTaxesResult(annualSalaryGross, taxResults.ToArray());
    }
    
    public AppliedTaxesResult UnapplyTaxes(decimal annualSalaryNet)
    {
        var min = annualSalaryNet;
        var max = annualSalaryNet * 3;

        var counter = 12;

        AppliedTaxesResult lastApplyTaxesResult;
        do
        {
            var guess = (min + max) / 2;

            lastApplyTaxesResult = ApplyTaxes(guess);

            if (lastApplyTaxesResult.SalaryNet > annualSalaryNet)
            {
                max = guess;
            }

            if (lastApplyTaxesResult.SalaryNet < annualSalaryNet)
            {
                min = guess;
            }
        }
        while(--counter > 0);
        
        return lastApplyTaxesResult;
    }
}