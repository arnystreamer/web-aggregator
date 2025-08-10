using System.Text.RegularExpressions;
using Jimx.WebAggregator.Calculations.Models;
using Jimx.WebAggregator.Domain.CityCosts;

namespace Jimx.WebAggregator.Calculations.Helpers;

public static class TaxFunctions
{
    public static bool IsTaxApplicable(UserTaxProfile userTaxProfile, IncomeTaxItem tax)
    {
        if (tax.IsActive == false)
        {
            return false;
        }

        return tax.Tags?.All(userTaxProfile.IsParameterMatched) ?? true;
    }
    
    public static TaxResult GetPersonTaxValue(IncomeTaxItem tax, int numberOfPeople)
    {
        var value = GetFixedTaxValue(tax).Value * numberOfPeople * (tax.Multiplier ?? 1);
        return new TaxResult(tax.Name, value);
    }
    
    public static TaxResult GetHousingTaxValue(IncomeTaxItem tax, int numberOfRentedHouses)
    {
        var value = GetFixedTaxValue(tax).Value * numberOfRentedHouses * (tax.Multiplier ?? 1);
        return new TaxResult(tax.Name, value);
    }
    
    public static TaxResult GetOwnedHousingTaxValue(IncomeTaxItem tax, int numberOfOwnedHouses)
    {
        var value = GetFixedTaxValue(tax).Value * numberOfOwnedHouses * (tax.Multiplier ?? 1);
        return new TaxResult(tax.Name, value);
    }

    public static TaxResult GetGrossTaxValue(IncomeTaxItem tax, decimal annualTaxBaseBeforeDeductions, Action<string, decimal> identifiedTaxSetter)
    {
        var taxBase = annualTaxBaseBeforeDeductions - (tax.FixedDeductionAmount ?? 0);
        
        var taxValueBeforeMultiplier = taxBase > 0 ? GetRatedTaxValue(tax, taxBase).Value : 0;
        if (!string.IsNullOrEmpty(tax.ID))
        {
            identifiedTaxSetter(tax.ID, taxValueBeforeMultiplier);
        }

        var value = taxValueBeforeMultiplier * (tax.Multiplier ?? 1);
        return new TaxResult(tax.Name, value);
    }

    public static TaxResult GetTaxOnTaxValue(IncomeTaxItem tax, Func<string, decimal> identifiedTaxGetter)
    {
        var regexMatch = Regex.Match(tax.ApplyOn!, @"tax\((\w+)\)");

        if (!regexMatch.Success || regexMatch.Groups.Count != 2)
        {
            throw new Exception($"ApplyOn format is incorrect, expected tax(ID), but received {tax.ApplyOn}");
        }

        var taxId = regexMatch.Groups[1].Value;
        var taxDeductionValue = identifiedTaxGetter(taxId);
        
        return GetRatedTaxValue(tax, taxDeductionValue * (tax.Multiplier ?? 1));
    }
    
    public static TaxResult GetFixedTaxValue(IncomeTaxItem tax)
    {
        if (tax.Rate.HasValue || (tax.Levels != null && tax.Levels.Length > 0))
        {
            throw new Exception("Fixed tax cannot have Rate or Levels");
        }

        if (tax.FixedDeductionAmount.HasValue)
        {
            throw new Exception("Fixed tax cannot have FixedDeductionAmount");
        }
        
        if (!(tax.MonthlyFixedAmount.HasValue ^ tax.AnnualFixedAmount.HasValue))
        {
            throw new Exception($"Either MonthlyFixedAmount ({tax.MonthlyFixedAmount}) or AnnualFixedAmount ({tax.AnnualFixedAmount}) should be specified for fixed tax");
        }

        var value = tax.MonthlyFixedAmount.HasValue 
            ? tax.MonthlyFixedAmount.Value * 12 
            : tax.AnnualFixedAmount!.Value;

        return new TaxResult(tax.Name, value);
    }

    public static TaxResult GetRatedTaxValue(IncomeTaxItem tax, decimal taxBase)
    {
        if (tax.MonthlyFixedAmount.HasValue || tax.AnnualFixedAmount.HasValue)
        {
            throw new Exception("Rated tax cannot have MonthlyFixedAmount or AnnualFixedAmount");
        }

        if (tax.Rate.HasValue && tax.Levels != null && tax.Levels.Length > 0)
        {
            throw new Exception("Rated tax cannot have both Rate and Levels");
        }
        
        if (!tax.Rate.HasValue && (tax.Levels == null || tax.Levels.Length == 0))
        {
            throw new Exception("Rated tax must have Rate or Levels");
        }

        var actualTaxBase = taxBase - (tax.FixedDeductionAmount ?? 0.0m);

        
        if (tax.Rate.HasValue)
        {
            var value = actualTaxBase * tax.Rate.Value;
            return new TaxResult(tax.Name, value);
        }

        return GetLeveledTaxValue(tax.Name, tax.Levels!, actualTaxBase);
    }

    public static TaxResult GetLeveledTaxValue(string taxName, IncomeTaxLevel[] levels, decimal taxBase)
    {
        var accumulatedTaxValue = 0.0m;
        
        foreach (var level in levels.Where(l => l.From < taxBase))
        {
            if (level.From > level.To)
            {
                throw new Exception("From cannot exceed To");
            }

            decimal? taxValueForThisLevel = null;
            
            var upperThresholdForThisLevel = Math.Min(level.To ?? decimal.MaxValue, taxBase);
            
            if (level.Rate.HasValue)
            {
                taxValueForThisLevel = (upperThresholdForThisLevel - level.From) * level.Rate.Value;
            }

            if (level.RateFrom.HasValue && level.RateTo.HasValue)
            {
                if (!level.To.HasValue)
                {
                    throw new Exception("To cannot be empty for linear tax");
                }
                
                var yDelta = level.RateTo.Value - level.RateFrom.Value;
                var xDelta = level.To.Value - level.From;
                
                var averageTax = (yDelta/xDelta * (upperThresholdForThisLevel - level.From) + 2.0m * level.RateFrom.Value) / 2.0m;

                taxValueForThisLevel = (upperThresholdForThisLevel - level.From) * averageTax;
            }

            if (!taxValueForThisLevel.HasValue)
            {
                throw new Exception("Failed to calculate tax for level because of rates: "+
                                    $"Rate {level.Rate}, RateFrom {level.RateFrom}, RateTo {level.RateTo}");
            }

            accumulatedTaxValue += taxValueForThisLevel.Value;
        }

        return new TaxResult(taxName, accumulatedTaxValue);
    }
    
    public static double GetMortgageMonthlyPayment(double interestYearlyPercentage, double years, double debtAmount)
    {
        var numberOfMonthlyPayments = years * 12.0;

        if (interestYearlyPercentage == 0.0)
        {
            return debtAmount / numberOfMonthlyPayments;
        }

        var monthlyInterestRate = interestYearlyPercentage / 100.0 / 12.0;
        return monthlyInterestRate * debtAmount * Math.Pow(1 + monthlyInterestRate, numberOfMonthlyPayments) /
               (Math.Pow(1 + monthlyInterestRate, numberOfMonthlyPayments) - 1);
    }
}