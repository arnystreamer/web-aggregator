using Jimx.WebAggregator.API.Models;
using Jimx.WebAggregator.API.Models.Common;
using Jimx.WebAggregator.Calculations.Helpers;

namespace Jimx.WebAggregator.API.Helpers;

public static class CityDataItemsHelper
{
    private static decimal? GetCost(this CityDataItemApi[] dataItems, CostCategory dictionaryId)
    {
        return dataItems.FirstOrDefault(di => di.DictionaryId == (int)dictionaryId)?.Value;
    }
    
    private static decimal GetCostOrDefault(this CityDataItemApi[] dataItems, CostCategory dictionaryId)
    {
        return GetCost(dataItems, dictionaryId) ?? 0.0m;
    }

    public static decimal GetGoingOutCosts(this CityDataItemApi[] dataItems)
    {
        return 
            GetCostOrDefault(dataItems, CostCategory.RestaurantInexpensive) * 8.00m +
            GetCostOrDefault(dataItems, CostCategory.RestaurantMidRange) * 6.00m +
            GetCostOrDefault(dataItems, CostCategory.RestaurantMcDonalds) * 4.00m +
            GetCostOrDefault(dataItems, CostCategory.RestaurantDomesticBeer) * 6.00m +
            GetCostOrDefault(dataItems, CostCategory.RestaurantImportedBeer) * 6.00m +
            GetCostOrDefault(dataItems, CostCategory.RestaurantCappuccino) * 24.00m +
            GetCostOrDefault(dataItems, CostCategory.RestaurantCoke033) * 8.00m +
            GetCostOrDefault(dataItems, CostCategory.RestaurantWater033) * 16.00m +
            GetCostOrDefault(dataItems, CostCategory.CinemaInternationalRelease) * 8.00m;
    }

    public static decimal GetGroceriesCosts(this CityDataItemApi[] dataItems, int numberOfPeople)
    {
        return
            GetCostOrDefault(dataItems, CostCategory.MarketMilk) * 8.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketWhiteBread) * 8.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketRice) * 4.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.Market12Eggs) * 8.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketLocalCheese) * 4.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketChickenFillets) * 6.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketBeefRound) * 6.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketApples) * 8.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketBanana) * 8.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketOranges) * 8.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketTomato) * 8.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketPotato) * 8.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketOnion) * 4.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketLettuce) * 4.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.MarketDomesticBeer) * 8.0m +
            GetCostOrDefault(dataItems, CostCategory.MarketImportedBeer) * 8.0m +
            GetCostOrDefault(dataItems, CostCategory.MarketBottleOfWine) * 4.0m +
            GetCostOrDefault(dataItems, CostCategory.Market15Water) * 12.0m * numberOfPeople;
    }

    public static decimal GetTransportationCosts(this CityDataItemApi[] dataItems, int numberOfPeople)
    {
        return
            GetCostOrDefault(dataItems, CostCategory.TransportOneWayTicket) * 8.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.TransportMonthlyPass) * 1.0m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.TransportTaxiStart) * 6.0m +
            GetCostOrDefault(dataItems, CostCategory.TransportTaxi1Km) * 6.0m * 10.0m +
            GetCostOrDefault(dataItems, CostCategory.TransportTaxi1HWaiting) * 6.0m * 0.15m +
            GetCostOrDefault(dataItems, CostCategory.TransportGasoline) * 90.0m +
            GetCostOrDefault(dataItems, CostCategory.TransportVolkswagenGolf) / 12.0m / 10.0m;
    }

    public static decimal GetUtilitiesCosts(this CityDataItemApi[] dataItems)
    {
        return
            GetCostOrDefault(dataItems, CostCategory.UtilitiesBasic) +
            GetCostOrDefault(dataItems, CostCategory.UtilitiesMobilePhone) * 2.0m +
            GetCostOrDefault(dataItems, CostCategory.UtilitiesInternet);
    }

    public static decimal GetSportsAndLeisureCosts(this CityDataItemApi[] dataItems)
    {
        return
            GetCostOrDefault(dataItems, CostCategory.SportsFitnessClubMonthly) * 2.0m +
            GetCostOrDefault(dataItems, CostCategory.SportsTennisCourtRent) * 2.0m;
    }

    public static decimal GetClothingCosts(this CityDataItemApi[] dataItems, int numberOfPeople)
    {
        return
            GetCostOrDefault(dataItems, CostCategory.ClothingPairOfJeans) * 0.2m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.ClothingSummerDress) * 0.2m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.ClothingRunningShoes) * 0.2m * numberOfPeople +
            GetCostOrDefault(dataItems, CostCategory.ClothingLeatherBusinessShoes) * 0.2m * numberOfPeople;
    }
    
    public static decimal GetRentCosts(this CityDataItemApi[] dataItems, bool isBigApt, bool isInCenter)
    {
        return (isBigApt, isInCenter) switch
        {
            (false, false) => GetCostOrDefault(dataItems, CostCategory.Rent1BedroomOutsideCentre),
            (false, true) => GetCostOrDefault(dataItems, CostCategory.Rent1BedroomInCentre),
            (true, false) => GetCostOrDefault(dataItems, CostCategory.Rent3BedroomOutsideCentre),
            (true, true) => GetCostOrDefault(dataItems, CostCategory.Rent3BedroomInCentre)
        };
    }

    public static decimal GetChildcareCosts(this CityDataItemApi[] dataItems, bool isChildPreschooler)
    {
        return isChildPreschooler 
            ? GetCostOrDefault(dataItems, CostCategory.ChildcareKindergartenMonthly) 
            : GetCostOrDefault(dataItems, CostCategory.ChildcareInternationalPrimarySchoolYearly) / 12.0m;
    }

    public static decimal GetApartmentPrice(this CityDataItemApi[] dataItems, bool isInCenter)
    {
        return isInCenter
            ? GetCostOrDefault(dataItems, CostCategory.InvestmentsPricePerSquareMeterInCenter) * 80.0m
            : GetCostOrDefault(dataItems, CostCategory.InvestmentsPricePerSquareMeterOutsideCenter) * 80.0m;
    }

    public static decimal GetCarPrice(this CityDataItemApi[] dataItems)
    {
        return GetCostOrDefault(dataItems, CostCategory.TransportVolkswagenGolf);
    }
    
    public static decimal GetInterestYearlyPercentage(this CityDataItemApi[] dataItems)
    {
        return GetCostOrDefault(dataItems, CostCategory.InvestmentsMortgageInterestPercentage);
    }
    
    public static decimal GetMortgageMonthlyPayment(this CityDataItemApi[] dataItems, 
        bool isApartmentInCenter, decimal pricesMultiplier, decimal downPaymentPart = 0.2m)
    {
        var interestYearlyPercentage = (double)GetInterestYearlyPercentage(dataItems);
        var loanAmount = (double)(GetApartmentPrice(dataItems, isApartmentInCenter) * pricesMultiplier * (1.0m - downPaymentPart));
        
        try
        {
            return (decimal)TaxFunctions.GetMortgageMonthlyPayment(
                interestYearlyPercentage,
                30.0,
                loanAmount);
        }
        catch (OverflowException overflowException)
        {
            throw new Exception($"Overflow. InterestYearlyPercentage = {interestYearlyPercentage}, years = 30, loanAmount = {loanAmount}", 
                overflowException);
        }
    }

    public static decimal GetMortgageDownPayment(this CityDataItemApi[] dataItems, 
        bool isApartmentInCenter, decimal downPaymentPart = 0.2m)
    {
        return GetApartmentPrice(dataItems, isApartmentInCenter) * downPaymentPart;
    }
    
    public static decimal? GetExpatNetSalary(this CityDataItemApi[] dataItems)
    {
        return GetCost(dataItems, CostCategory.InvestmentsAverageMonthlyNetSalary);
    }
}