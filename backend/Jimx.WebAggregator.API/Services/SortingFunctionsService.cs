using Jimx.WebAggregator.API.Models.CityCosts;
using Jimx.WebAggregator.API.Models.Report;

namespace Jimx.WebAggregator.API.Services;

public class SortingFunctionsService
{
    private readonly SortingFunction[] _sortingFunctions =
    [
        new(1, "Net selected salary in USD", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SelectedSalary.ValueNet)),
        new(2, "Gross selected salary in USD", string.Empty,
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SelectedSalary.ValueGross)),
        
        new(10, "YearsToGet1MlnUsd", string.Empty,
            FieldAbsoluteSortingComparerFactory.Create(x => x.MillionaireTerm.TermInMonths ?? Decimal.MaxValue)),
        new(11, "YearsToGetMortgageDownPayment", string.Empty,
            FieldAbsoluteSortingComparerFactory.Create(x => x.MortgageDownPaymentTerm.TermInMonths ?? Decimal.MaxValue)),
        new(12, "YearsToGetCar", string.Empty,
            FieldAbsoluteSortingComparerFactory.Create(x => x.BuyCarTerm.TermInMonths ?? Decimal.MaxValue)),
        
        new(20, "SalaryToEarn1MlnUsdIn30Yrs", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryToEarn1MlnUsdIn30Yrs.ValueGross)),
        new(21, "SustainableSalary", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SustainableSalary.ValueGross)),
        new(22, "BareMinimumSalary", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.BareMinimumSalary.ValueGross)),

        new(31, "MinimumCostsWithRent", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.MinimumCostsWithRent.ValueNet)),
        new(32, "CostsWithRent", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.CostsWithRent.ValueNet)),
        new(33, "CostsWithMortgage", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.CostsWithMortgage.ValueNet)),
        new(34, "SavingsWhileRenting", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.MonthlySavingsWhileRenting)),
        new(35, "SavingsWhilePayingMortgage", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.MonthlySavingsWhilePayingMortgage)),
        
        
        new(40, "AverageSalary ValueGross", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryData.AverageSalary.ValueGross)),
        new(41, "AverageSalary ValueNet", string.Empty,
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryData.AverageSalary.ValueNet)),
        new(42, "P25Salary ValueGross", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryData.P25Salary.ValueGross)),
        new(43, "P25Salary ValueNet", string.Empty,
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryData.P25Salary.ValueNet)),
        new(44, "P75Salary ValueGross", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryData.P75Salary.ValueGross)),
        new(45, "P75Salary ValueNet", string.Empty,
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryData.P75Salary.ValueNet))
    ];
    
    public SortingFunction[] GetAll()
    {
        return _sortingFunctions;
    }

    public SortingFunction Get(int id)
    {
        return _sortingFunctions.Single(s => s.Id == id);
    }
    
    public class Comparer(Func<ReportCityExtendedApi, ReportCityExtendedApi, int> comparerFunction) 
        : IComparer<ReportCityExtendedApi>
    {
        public int Compare(ReportCityExtendedApi? x, ReportCityExtendedApi? y)
        {
            if (x == null && y == null)
                return 0;

            if (x == null)
                return -1;
            
            if (y == null)
                return 1;
            
            return comparerFunction(x, y);
        }
    }
    
    public class FieldAbsoluteComparer(Func<ReportCityExtendedApi, decimal> valueSelector, SortingDirection sortingDirection) 
        : Comparer((r1, r2) => CompareDecimalFields(valueSelector, r1, r2, sortingDirection))
    {
        public static int CompareDecimalFields(Func<ReportCityExtendedApi, decimal> valueSelector, 
            ReportCityExtendedApi x, ReportCityExtendedApi y, SortingDirection sortingDirection)
        {
            var comparingValue = valueSelector(x) - valueSelector(y);

            if (comparingValue == 0)
                return 0;

            return (comparingValue > 0 ? 1 : -1) * (sortingDirection == SortingDirection.Ascending ? 1 : -1);
        }
    }
    
    public static class FieldAbsoluteSortingComparerFactory
    {
        public static Func<SortingDirection, IComparer<ReportCityExtendedApi>> Create(Func<ReportCityExtendedApi, decimal> valueSelector)
        {
            return direction => new FieldAbsoluteComparer(valueSelector, direction);
        }
    }
    
    public class FieldAbsoluteInUsdComparer(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector, SortingDirection sortingDirection) 
        : Comparer((r1, r2) => CompareMultiCurrencyValueFields(valueSelector, r1, r2, sortingDirection))
    {
        public static int CompareMultiCurrencyValueFields(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector, 
            ReportCityExtendedApi x, ReportCityExtendedApi y, SortingDirection sortingDirection)
        {
            var comparingValue = valueSelector(x).ValueInUsd - valueSelector(y).ValueInUsd;

            if (comparingValue == 0)
                return 0;

            return (comparingValue > 0 ? 1 : -1) * (sortingDirection == SortingDirection.Ascending ? 1 : -1);
        }
    }

    public static class FieldAbsoluteInUsdSortingComparerFactory
    {
        public static Func<SortingDirection, IComparer<ReportCityExtendedApi>> Create(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector)
        {
            return direction => new FieldAbsoluteInUsdComparer(valueSelector, direction);
        }
    }
}