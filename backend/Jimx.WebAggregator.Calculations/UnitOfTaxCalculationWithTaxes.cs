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
        
        for (var i = 0; i < _taxDeductions.Length; i++)
        {
            var taxes = _taxDeductions[i].IncomeTaxes.Where(tax => TaxFunctions.IsTaxApplicable(_userTaxProfile, tax))
                .ToArray();
            
            var deductionsById = new Dictionary<string, decimal>();

            var beforeTaxesTaxes = taxes.Where(tax => tax.ApplyOn == "before_taxes").ToArray();
            if (beforeTaxesTaxes.Length > 1)
            {
                throw new Exception("More than one tax with type before_taxes applied");
            }

            var innerTaxResults = new List<TaxResult>();
            if (beforeTaxesTaxes.Length == 1)
            {
                innerTaxResults.Add(TaxFunctions.GetGrossTaxValue(beforeTaxesTaxes[0], annualSalaryGross, (id, val) => deductionsById.Add(id, val)));
            }

            var grossTaxBase = annualSalaryGross - innerTaxResults.SumOrDefault(r => r.Value);
            
            var nonDependantGrossTaxes = taxes.Where(tax => string.IsNullOrEmpty(tax.ApplyOn) || tax.ApplyOn == "gross").ToArray();
            innerTaxResults.AddRange(nonDependantGrossTaxes.Select(tax => 
                TaxFunctions.GetGrossTaxValue(tax, grossTaxBase, (id, val) => deductionsById.Add(id, val))));
            
            var dependantGrossTaxes = taxes.Where(tax => !string.IsNullOrEmpty(tax.ApplyOn) && tax.ApplyOn.StartsWith("tax")).ToArray();
            innerTaxResults.AddRange(dependantGrossTaxes.Select(tax => TaxFunctions.GetTaxOnTaxValue(tax, taxId => deductionsById[taxId])));
            
            var housingTaxes = taxes.Where(tax => tax.ApplyOn == "housing").ToArray();
            innerTaxResults.AddRange(housingTaxes.Select(tax => TaxFunctions.GetHousingTaxValue(tax, 1)));
            
            var personTaxes = taxes.Where(tax => tax.ApplyOn == "person").ToArray();
            innerTaxResults.AddRange(personTaxes.Select(tax => TaxFunctions.GetPersonTaxValue(tax, 1)));
            
            var ownedHousingTaxes = taxes.Where(tax => tax.ApplyOn == "owned_housing").ToArray();
            innerTaxResults.AddRange(ownedHousingTaxes.Select(tax => TaxFunctions.GetOwnedHousingTaxValue(tax, 0)));
            
            var afterTaxesTaxes = taxes.Where(tax => tax.ApplyOn == "after_taxes").ToArray();
            if (afterTaxesTaxes.Length > 1)
            {
                throw new Exception("More than one tax with type after_taxes applied");
            }

            var afterTaxesBase = annualSalaryGross - innerTaxResults.SumOrDefault(r => r.Value);
            if (afterTaxesTaxes.Length == 1)
            {
                innerTaxResults.AddRange(TaxFunctions.GetGrossTaxValue(afterTaxesTaxes[0], afterTaxesBase, (id, val) => deductionsById.Add(id, val)));
            }
            
            taxResults.AddRange(innerTaxResults);
        }

        return new AppliedTaxesResult(annualSalaryGross, taxResults.ToArray());
    }
    
    public AppliedTaxesResult UnapplyTaxes(decimal annualSalaryNet)
    {
        var min = annualSalaryNet;
        var max = annualSalaryNet * 3;

        var counter = 12;
        decimal guess;

        AppliedTaxesResult lastApplyTaxesResult;
        do
        {
            guess = (min + max) / 2;

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