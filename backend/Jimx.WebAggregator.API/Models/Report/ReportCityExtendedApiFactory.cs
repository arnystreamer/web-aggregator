using Jimx.WebAggregator.API.Helpers;
using Jimx.WebAggregator.API.Models.CityCosts;
using Jimx.WebAggregator.Calculations;
using Jimx.WebAggregator.Calculations.Models;
using Jimx.WebAggregator.Domain.CityCosts;

namespace Jimx.WebAggregator.API.Models.Report;

public class ReportCityExtendedApiFactory
{
    private readonly UnitOfTaxCalculation _calculation;

    public ReportCityExtendedApiFactory(UnitOfTaxCalculation calculation)
    {
        _calculation = calculation;
    }

    public ReportCityExtendedApi Create(ReportCityApi reportCityApi, 
        CityDataItemApi[] dataItems, RegionTaxDeduction[] taxDeductions,
        ReportCityDynamicParameters dynamicParameters)
    {
        var taxesCalculator = _calculation.WithTaxes(taxDeductions);

        var hasFreeApartment = dynamicParameters.ApartmentParameters.HasFlag(ReportApartmentParameters.Free);

        var pricesMultiplier = 1m;
        if (dynamicParameters.ArePricesNominatedInUsd)
        {
            pricesMultiplier = 1m / dynamicParameters.CrossRateToUsd;
        }
        
        var goingOutCosts = dataItems.GetGoingOutCosts() * pricesMultiplier;
        var groceriesCosts = dataItems.GetGroceriesCosts(dynamicParameters.HouseholdMembers.TotalCount) * pricesMultiplier;
        var householdCosts = groceriesCosts * 0.05m;
        var transportationCosts =
            dataItems.GetTransportationCosts(dynamicParameters.HouseholdMembers.TotalCount -
                                             dynamicParameters.HouseholdMembers.Toddlers) * pricesMultiplier;
        var utilitiesCosts = dataItems.GetUtilitiesCosts() * pricesMultiplier;
        var sportsAndLeisureCosts = dataItems.GetSportsAndLeisureCosts() * pricesMultiplier;
        var clothingCosts = dataItems.GetClothingCosts(dynamicParameters.HouseholdMembers.TotalCount) * pricesMultiplier;
        var rentCosts = dataItems.GetRentCosts(
            dynamicParameters.ApartmentParameters.HasFlag(ReportApartmentParameters.Big), 
            dynamicParameters.ApartmentParameters.HasFlag(ReportApartmentParameters.Center)) * pricesMultiplier;
        var mortgageMonthlyPayment = dataItems.GetMortgageMonthlyPayment(
            dynamicParameters.ApartmentParameters.HasFlag(ReportApartmentParameters.Center),
            pricesMultiplier);
        var childcareCosts = dataItems.GetChildcareCosts(true) * dynamicParameters.HouseholdMembers.Prescholars * pricesMultiplier +
                             dataItems.GetChildcareCosts(false) * dynamicParameters.HouseholdMembers.Scholars * pricesMultiplier;
        var vacationCosts = 200m * dynamicParameters.HouseholdMembers.TotalCount / dynamicParameters.CrossRateToUsd;
        var electronicsCosts = 40.0m * (dynamicParameters.HouseholdMembers.TotalCount + 1) / dynamicParameters.CrossRateToUsd;
        
        var selectedSalaryNet = new ReportProfitTaxable(taxesCalculator.ApplyTaxes(dynamicParameters.SelectedSalary), dynamicParameters.CrossRateToUsd);

        var costBitsWithoutRent = new[]
        {
            new CostBit("Going out", goingOutCosts),
            new CostBit("Groceries", groceriesCosts),
            new CostBit("Household", householdCosts),
            new CostBit("Transportation", transportationCosts),
            new CostBit("Utilities", utilitiesCosts),
            new CostBit("Sports", sportsAndLeisureCosts),
            new CostBit("Clothing", clothingCosts),
            new CostBit("Vacation", vacationCosts),
            new CostBit("Electronics", electronicsCosts)
        };
        
        var minimumCostsBitsWithoutRent = new[]
        {
            new CostBit("Going out", goingOutCosts * 0.5m),
            new CostBit("Groceries", groceriesCosts),
            new CostBit("Household", householdCosts),
            new CostBit("Transportation", transportationCosts * 0.5m),
            new CostBit("Utilities", utilitiesCosts),
            new CostBit("Sports", sportsAndLeisureCosts * 0.5m),
            new CostBit("Clothing", clothingCosts * 0.8m),
            new CostBit("Vacation", vacationCosts * 0.5m),
            new CostBit("Electronics", electronicsCosts)
        };

        CostBit[] costBitsWithRent =
        [
            ..costBitsWithoutRent,
            new CostBit("Rent", hasFreeApartment ? 0m : rentCosts)
        ];
        var costsWithRent = new ReportCostRelativeExtended(costBitsWithRent,
            dynamicParameters.CrossRateToUsd, selectedSalaryNet);
        
        CostBit[] minimumCostBitsWithRent =
        [
            ..minimumCostsBitsWithoutRent,
            new CostBit("Rent", hasFreeApartment ? 0m : rentCosts)
        ];
        var minimumCostsWithRent = new ReportCostRelativeExtended(minimumCostBitsWithRent,
            dynamicParameters.CrossRateToUsd, selectedSalaryNet);
        
        CostBit[] costBitsWithMortgage =
        [
            ..costBitsWithoutRent,
            new CostBit("Mortgage", mortgageMonthlyPayment)
        ];
        var costsWithMortgage = new ReportCostRelativeExtended(costBitsWithMortgage,
            dynamicParameters.CrossRateToUsd, selectedSalaryNet);

        var monthlySavingsWhileRenting = selectedSalaryNet.ValueNet.Value / 12m - costsWithRent.ValueNet.Value;
        var monthlySavingsWhileRentingInUsd = monthlySavingsWhileRenting * dynamicParameters.CrossRateToUsd;
        
        var monthlySavingsWhilePayingMortgage = selectedSalaryNet.ValueNet.Value / 12m - costsWithMortgage.ValueNet.Value;
        var monthlySavingsWhilePayingMortgageInUsd = monthlySavingsWhilePayingMortgage * dynamicParameters.CrossRateToUsd;

        var millionaireTarget = new MultiCurrencyValue(1_000_000m / dynamicParameters.CrossRateToUsd, 1_000_000m);
        var millionaireTerm = new ReportProfitTargetTerm(
            selectedSalaryNet,
            costsWithRent,
            millionaireTarget,
            monthlySavingsWhileRenting > 0 ? millionaireTarget.Value / monthlySavingsWhileRenting : null);

        var mortgageDownPayment = dataItems.GetMortgageDownPayment(dynamicParameters.ApartmentParameters.HasFlag(ReportApartmentParameters.Center))
                                  * pricesMultiplier;
        var mortgageDownPaymentTarget = new MultiCurrencyValue(mortgageDownPayment, mortgageDownPayment * dynamicParameters.CrossRateToUsd);
        var mortgageDownPaymentTerm = new ReportProfitTargetTerm(
            selectedSalaryNet,
            costsWithRent,
            mortgageDownPaymentTarget,
            monthlySavingsWhileRenting > 0 ? mortgageDownPaymentTarget.Value / monthlySavingsWhileRenting : null);

        var carPrice = dataItems.GetCarPrice() * pricesMultiplier;
        var carPriceTarget = new MultiCurrencyValue(carPrice, carPrice * dynamicParameters.CrossRateToUsd);
        var buyCarTerm = new ReportProfitTargetTerm(
            selectedSalaryNet,
            costsWithRent,
            carPriceTarget,
            monthlySavingsWhileRenting > 0 ? carPriceTarget.Value / monthlySavingsWhileRenting : null);

        var millionaireTargetAnnualSavingsInUsd = 1_000_000m / 30m;
        var netAnnualSalaryToMillionaireInUsd = millionaireTargetAnnualSavingsInUsd + costsWithRent.ValueNet.ValueInUsd * 12m;
        var netAnnualSalaryToMillionaire = netAnnualSalaryToMillionaireInUsd / dynamicParameters.CrossRateToUsd;
        var desirableSalaryObjectToMillionaire = new ReportProfitTaxableRelativeDesirable(
            taxesCalculator.UnapplyTaxes(netAnnualSalaryToMillionaire),
            dynamicParameters.CrossRateToUsd, 
            dynamicParameters.SelectedSalary,
            new MultiCurrencyValue(netAnnualSalaryToMillionaire, netAnnualSalaryToMillionaireInUsd),
            costBitsWithRent,
            new MultiCurrencyValue(millionaireTargetAnnualSavingsInUsd / dynamicParameters.CrossRateToUsd, millionaireTargetAnnualSavingsInUsd));

        var sustainableAnnualSavingsInUsd = 15000m;
        var netSustainableAnnualSalaryInUsd = costsWithRent.ValueNet.ValueInUsd * 12m + sustainableAnnualSavingsInUsd;
        var netSustainableAnnualSalary = netSustainableAnnualSalaryInUsd / dynamicParameters.CrossRateToUsd;
        var desirableSalaryObjectToSustain = new ReportProfitTaxableRelativeDesirable(
            taxesCalculator.UnapplyTaxes(netSustainableAnnualSalary),
            dynamicParameters.CrossRateToUsd, 
            dynamicParameters.SelectedSalary,
            new MultiCurrencyValue(netSustainableAnnualSalary, netSustainableAnnualSalaryInUsd),
            costBitsWithRent, 
            new MultiCurrencyValue(sustainableAnnualSavingsInUsd / dynamicParameters.CrossRateToUsd, sustainableAnnualSavingsInUsd));
        
        var bareMinimumAnnualSalary = minimumCostsWithRent.ValueNet.Value * 12m;
        var bareMinimumAnnualSalaryInUsd = minimumCostsWithRent.ValueNet.ValueInUsd * 12m;
        var desirableSalaryObjectToSurvive = new ReportProfitTaxableRelativeDesirable(
                taxesCalculator.UnapplyTaxes(bareMinimumAnnualSalary),
                dynamicParameters.CrossRateToUsd,
                dynamicParameters.SelectedSalary,
                new MultiCurrencyValue(bareMinimumAnnualSalary, bareMinimumAnnualSalaryInUsd),
                minimumCostBitsWithRent,
                new MultiCurrencyValue(0m, 0m)); 
        
        return new ReportCityExtendedApi(reportCityApi.Name, reportCityApi.Region, reportCityApi.Country, reportCityApi.CountryCode)
        {
            SelectedSalary = new ReportProfitTaxable(taxesCalculator.ApplyTaxes(dynamicParameters.SelectedSalary), dynamicParameters.CrossRateToUsd),
            SalaryData = new SalaryData()
            {
                AverageSalary = new ReportProfitTaxable(taxesCalculator.ApplyTaxes(dynamicParameters.AnnualGrossSalary), dynamicParameters.CrossRateToUsd),
                P25Salary = new ReportProfitTaxable(taxesCalculator.ApplyTaxes(dynamicParameters.DeveloperGrossSalaryP25), dynamicParameters.CrossRateToUsd),
                P75Salary = new ReportProfitTaxable(taxesCalculator.ApplyTaxes(dynamicParameters.DeveloperGrossSalaryP75), dynamicParameters.CrossRateToUsd)
            },
            CurrencyCode = dynamicParameters.CurrencyCode,
            HasFreeApartment = hasFreeApartment,
            GoingOutCosts = goingOutCosts,
            GroceriesCosts = groceriesCosts,
            HouseholdCosts = householdCosts,
            TransportationCosts = transportationCosts,
            UtilitiesCosts = utilitiesCosts,
            SportsAndLeisureCosts = sportsAndLeisureCosts,
            ClothingCosts = clothingCosts,
            RentCosts = rentCosts,
            MortgageCosts = mortgageMonthlyPayment,
            ChildcareCosts = childcareCosts,
            VacationCosts = vacationCosts,
            ElectronicsCosts = electronicsCosts,

            MinimumCostsWithRent = minimumCostsWithRent,
            CostsWithRent = costsWithRent,
            CostsWithMortgage = costsWithMortgage,
            
            MonthlySavingsWhileRenting = new MultiCurrencyValue(monthlySavingsWhileRenting, monthlySavingsWhileRentingInUsd),
            MonthlySavingsWhilePayingMortgage = new MultiCurrencyValue(monthlySavingsWhilePayingMortgage, monthlySavingsWhilePayingMortgageInUsd),
            
            MillionaireTerm = millionaireTerm,
            MortgageDownPaymentTerm = mortgageDownPaymentTerm,
            BuyCarTerm = buyCarTerm,
            
            SalaryToEarn1MlnUsdIn30Yrs = desirableSalaryObjectToMillionaire,
            SustainableSalary = desirableSalaryObjectToSustain,
            BareMinimumSalary = desirableSalaryObjectToSurvive
        };
    }
}