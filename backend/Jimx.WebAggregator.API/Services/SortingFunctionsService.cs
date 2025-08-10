using Jimx.WebAggregator.API.Models.Report;

namespace Jimx.WebAggregator.API.Services;

public class SortingFunctionsService
{
    private readonly SortingFunction[] _sortingFunctions =
    [
        new(100, "MillionaireTerm", "Term to become millionaire", string.Empty,
            FieldAbsoluteSortingComparerFactory.Create(x => x.MillionaireTerm.TermInMonths ?? decimal.MaxValue)),
        new(110, "MortgageDownPaymentTerm", "Term to accumulate mortgage payment", string.Empty,
            FieldAbsoluteSortingComparerFactory.Create(x => x.MortgageDownPaymentTerm.TermInMonths ?? decimal.MaxValue)),
        new(120, "BuyCarTerm", "Term to buy car", string.Empty,
            FieldAbsoluteSortingComparerFactory.Create(x => x.BuyCarTerm.TermInMonths ?? decimal.MaxValue)),
        
        new(200, "MillionaireSalary", "Gross millionaire salary (absolute)", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.MillionaireSalary.ValueGross)),
        new(201, "MillionaireSalaryToSelectedSalary", "Gross millionaire salary - Selected salary", string.Empty, 
            FieldToGrossSelectedSalarySortingComparerFactory.Create(x => x.MillionaireSalary.ValueGross)),
        new(202, "MillionaireSalaryRelatedToSelectedSalary", "Gross millionaire salary / Selected salary", string.Empty,
            FieldRelatesToGrossSelectedSalarySortingComparerFactory.Create(x => x.MillionaireSalary.ValueGross)),
        
        new(210, "SustainableSalary", "Gross sustainable salary (absolute)", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SustainableSalary.ValueGross)),
        new(211, "SustainableSalaryToSelectedSalary", "Gross sustainable salary - Selected salary", string.Empty, 
            FieldToGrossSelectedSalarySortingComparerFactory.Create(x => x.SustainableSalary.ValueGross)),
        new(212, "SustainableSalaryRelatedToSelectedSalary", "Gross sustainable salary / Selected salary", string.Empty, 
            FieldRelatesToGrossSelectedSalarySortingComparerFactory.Create(x => x.SustainableSalary.ValueGross)),
        
        new(220, "BareMinimumSalary", "Gross bare minimum salary", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.BareMinimumSalary.ValueGross)),
        new(221, "BareMinimumSalaryToSelectedSalary", "Gross bare minimum salary - Selected salary", string.Empty, 
            FieldToGrossSelectedSalarySortingComparerFactory.Create(x => x.BareMinimumSalary.ValueGross)),
        new(222, "BareMinimumSalaryRelatesToSelectedSalary", "Gross bare minimum salary / Selected salary", string.Empty, 
            FieldRelatesToGrossSelectedSalarySortingComparerFactory.Create(x => x.BareMinimumSalary.ValueGross)),

        new(300, "MinimumCostsWithRent", "Minimum possible costs while renting", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.MinimumCostsWithRent.ValueNet)),
        new(301, "MinimumCostsWithRentToSelectedSalary", "Minimum possible costs while renting - Selected salary", string.Empty, 
            FieldToNetSelectedSalarySortingComparerFactory.Create(x => x.MinimumCostsWithRent.ValueNet.ApplyMultiplicator(12m))),
        new(302, "MinimumCostsWithRentRelatesToSelectedSalary", "Minimum possible costs while renting / Selected salary", string.Empty, 
            FieldRelatesToNetSelectedSalarySortingComparerFactory.Create(x => x.MinimumCostsWithRent.ValueNet.ApplyMultiplicator(12m))),
        
        new(310, "CostsWithRent", "Usual costs while renting", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.CostsWithRent.ValueNet)),
        new(311, "CostsWithRentToSelectedSalary", "Usual costs while renting - Selected salary", string.Empty, 
            FieldToNetSelectedSalarySortingComparerFactory.Create(x => x.CostsWithRent.ValueNet.ApplyMultiplicator(12m))),
        new(312, "CostsWithRentRelatesToSelectedSalary", "Usual costs while renting / Selected salary", string.Empty, 
            FieldRelatesToNetSelectedSalarySortingComparerFactory.Create(x => x.CostsWithRent.ValueNet.ApplyMultiplicator(12m))),
        
        
        new(320, "CostsWithMortgage", "Usual costs while paying mortgage", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.CostsWithMortgage.ValueNet)),
        new(321, "CostsWithMortgageToSelectedSalary", "Usual costs while paying mortgage - Selected salary", string.Empty,
            FieldToNetSelectedSalarySortingComparerFactory.Create(x => x.CostsWithMortgage.ValueNet.ApplyMultiplicator(12m))),
        new(322, "CostsWithMortgageRelatesToSelectedSalary", "Usual costs while paying mortgage / Selected salary", string.Empty, 
            FieldRelatesToNetSelectedSalarySortingComparerFactory.Create(x => x.CostsWithMortgage.ValueNet.ApplyMultiplicator(12m))),
        
        new(400, "MonthlySavingsWhileRenting", "Savings while renting", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.MonthlySavingsWhileRenting)),
        new(410, "MonthlySavingsWhilePayingMortgage", "Savings while paying mortgage", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.MonthlySavingsWhilePayingMortgage)),
        
        
        new(1000, "AverageGrossSalary", "Gross average salary", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryData.AverageSalary.ValueGross)),
        new(1001, "AverageNetSalary", "Net average salary", string.Empty,
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryData.AverageSalary.ValueNet)),
        new(1002, "P25GrossSalary", "Gross P25 salary", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryData.P25Salary.ValueGross)),
        new(1003, "P25NetSalary", "Net P25 salary", string.Empty,
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryData.P25Salary.ValueNet)),
        new(1004, "P75GrossSalary", "Gross P75 salary", string.Empty, 
            FieldAbsoluteInUsdSortingComparerFactory.Create(x => x.SalaryData.P75Salary.ValueGross)),
        new(1005, "P75NetSalary", "Net P75 salary", string.Empty,
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

        public static int CompareDifference(decimal difference, SortingDirection sortingDirection)
        {
            if (difference == 0m)
                return 0;

            return (difference > 0m ? 1 : -1) * (sortingDirection == SortingDirection.Ascending ? 1 : -1);
        }
    }
    
    public class FieldAbsoluteComparer(Func<ReportCityExtendedApi, decimal> valueSelector, SortingDirection sortingDirection) 
        : Comparer((r1, r2) => Compare(valueSelector, r1, r2, sortingDirection))
    {
        public static int Compare(Func<ReportCityExtendedApi, decimal> valueSelector, 
            ReportCityExtendedApi x, ReportCityExtendedApi y, SortingDirection sortingDirection)
        {
            var comparingValue = valueSelector(x) - valueSelector(y);
            return CompareDifference(comparingValue, sortingDirection);
            
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
        : Comparer((r1, r2) => Compare(valueSelector, r1, r2, sortingDirection))
    {
        public static int Compare(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector, 
            ReportCityExtendedApi x, ReportCityExtendedApi y, SortingDirection sortingDirection)
        {
            var comparingValue = valueSelector(x).ValueInUsd - valueSelector(y).ValueInUsd;
            return CompareDifference(comparingValue, sortingDirection);
        }
    }
    
    public static class FieldAbsoluteInUsdSortingComparerFactory
    {
        public static Func<SortingDirection, IComparer<ReportCityExtendedApi>> Create(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector)
        {
            return direction => new FieldAbsoluteInUsdComparer(valueSelector, direction);
        }
    }
    
    public abstract class FieldToFieldComparer(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector, 
        Func<ReportCityExtendedApi, MultiCurrencyValue> baseValueSelector, SortingDirection sortingDirection)
        :Comparer((r1, r2) => Compare(valueSelector, baseValueSelector, r1, r2, sortingDirection))
    {
        public static int Compare(
            Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector,
            Func<ReportCityExtendedApi, MultiCurrencyValue> baseValueSelector,
            ReportCityExtendedApi x, ReportCityExtendedApi y, SortingDirection sortingDirection)
        {
            var comparingValue = baseValueSelector(x).ValueInUsd - valueSelector(x).ValueInUsd -
                                 (baseValueSelector(y).ValueInUsd - valueSelector(y).ValueInUsd);
            return CompareDifference(comparingValue, sortingDirection);
        }
    }
    
    public abstract class FieldRelatesToFieldComparer(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector, 
        Func<ReportCityExtendedApi, MultiCurrencyValue> baseValueSelector, SortingDirection sortingDirection)
        :Comparer((r1, r2) => Compare(valueSelector, baseValueSelector, r1, r2, sortingDirection))
    {
        public static int Compare(
            Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector,
            Func<ReportCityExtendedApi, MultiCurrencyValue> baseValueSelector,
            ReportCityExtendedApi x, ReportCityExtendedApi y, SortingDirection sortingDirection)
        {
            const decimal veryBigValue = 1_000_000_000_000m;
            var usdValueX = valueSelector(x).ValueInUsd;
            var usdValueY = valueSelector(y).ValueInUsd;

            var comparingValue = (usdValueX > 0 ? baseValueSelector(x).ValueInUsd / usdValueX : veryBigValue) -
                                 (usdValueY > 0 ? baseValueSelector(y).ValueInUsd / usdValueY : veryBigValue);
            
            return CompareDifference(comparingValue, sortingDirection);
        }
    }

    public class FieldToGrossSelectedSalaryComparer(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector, SortingDirection sortingDirection)
        :FieldToFieldComparer(
            valueSelector, 
            x => x.SelectedSalary.ValueGross, 
            sortingDirection);
    
    public class FieldToNetSelectedSalaryComparer(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector, SortingDirection sortingDirection)
        :FieldToFieldComparer(
            valueSelector, 
            x => x.SelectedSalary.ValueNet, 
            sortingDirection);
    
    public class FieldRelatesToGrossSelectedSalaryComparer(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector, SortingDirection sortingDirection)
        :FieldRelatesToFieldComparer(
            valueSelector, 
            x => x.SelectedSalary.ValueGross, 
            sortingDirection);
    
    public class FieldRelatesToNetSelectedSalaryComparer(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector, SortingDirection sortingDirection)
        :FieldRelatesToFieldComparer(
            valueSelector, 
            x => x.SelectedSalary.ValueNet, 
            sortingDirection);

    public static class FieldToGrossSelectedSalarySortingComparerFactory
    {
        public static Func<SortingDirection, IComparer<ReportCityExtendedApi>> Create(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector)
        {
            return direction => new FieldToGrossSelectedSalaryComparer(valueSelector, direction);
        }
    }
    
    public static class FieldToNetSelectedSalarySortingComparerFactory
    {
        public static Func<SortingDirection, IComparer<ReportCityExtendedApi>> Create(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector)
        {
            return direction => new FieldToNetSelectedSalaryComparer(valueSelector, direction);
        }
    }
    
    public static class FieldRelatesToGrossSelectedSalarySortingComparerFactory
    {
        public static Func<SortingDirection, IComparer<ReportCityExtendedApi>> Create(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector)
        {
            return direction => new FieldRelatesToGrossSelectedSalaryComparer(valueSelector, direction);
        }
    }
    
    public static class FieldRelatesToNetSelectedSalarySortingComparerFactory
    {
        public static Func<SortingDirection, IComparer<ReportCityExtendedApi>> Create(Func<ReportCityExtendedApi, MultiCurrencyValue> valueSelector)
        {
            return direction => new FieldRelatesToNetSelectedSalaryComparer(valueSelector, direction);
        }
    }
}