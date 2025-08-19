using Jimx.WebAggregator.API.Helpers;
using Jimx.WebAggregator.API.Models;
using Jimx.WebAggregator.API.Models.Report;
using Jimx.WebAggregator.Calculations;
using Jimx.WebAggregator.Domain.CityCosts;

namespace Jimx.WebAggregator.API.Services;

public class ReportService
{
    private readonly string[] _citiesWithFreeApartment = ["Moscow"];
    
    private readonly string[] _countriesNominatedInUsd = ["Argentina"];
    
    private readonly ILogger<ReportService> _logger;
    private readonly CitiesDatabaseService _databaseService;
    private readonly TaxationService _taxationService;
    private readonly CrossRatesService _crossRatesService;

    public ReportService(ILogger<ReportService> logger, CitiesDatabaseService databaseService, TaxationService taxationService,
        CrossRatesService crossRatesService)
    {
        _logger = logger;
        _databaseService = databaseService;
        _taxationService = taxationService;
        _crossRatesService = crossRatesService;
    }

    public async Task<ReportCityExtendedApi[]> Get(int salaryTypeId, decimal? manualSalary, decimal? salaryMultiplicator, 
        SortingFunction sortingFunction, SortingDirection sortingDirection, UserTaxProfile userTaxProfile, CancellationToken cancellationToken)
    {
        var cityCostsItems = await _databaseService.GetCityCostsAsync(cancellationToken);
        var citySalaries = await _databaseService.GetCitySalariesAsync(cancellationToken);
        var taxDeductions = await _databaseService.GetRegionTaxDeductionsAsync(cancellationToken);

        var reportCityItemFactory = new ReportCityExtendedApiFactory(_taxationService.GetCalculation(userTaxProfile));
        
        var reportCityExtendedApis = new List<ReportCityExtendedApi>(cityCostsItems.Count);

        var differErrors = new List<string>(); 
        foreach (var cityCosts in cityCostsItems)
        {
            var countryName = cityCosts.Country;
            
            var dataItems = cityCosts.DataItems.Select(di => new CityDataItemApi(di.Key, di.DictionaryId, di.Value))
                .ToArray();
            
            var cityName = cityCosts.Name;
            
            var taxDeductionItems = taxDeductions.Where(d => d.Country == cityCosts.Country && (
                d.Region == null || d.Region == cityCosts.Region)).ToArray();
            
            var countryCode = taxDeductionItems.FirstOrDefault(d => d.Region == null)?.ID ?? null;
            var currencyCode = taxDeductionItems.First().CurrencyCode;
            var crossRateToUsd = _crossRatesService.Get(currencyCode);
            
            var citySalary = citySalaries.SingleOrDefault(cs => cs.City == cityName);
            if (citySalary == null || citySalary.Items.Length == 0)
            {
                throw new Exception($"Salary not specified for {cityName}");
            }

            var arePricesNominatedInUsd = _countriesNominatedInUsd.Contains(countryName);
            var averageExpatNetSalary = dataItems.GetExpatNetSalary();
            if (!averageExpatNetSalary.HasValue)
            {
                throw new Exception($"Expat net salary not specified for {cityName}");
            }

            var averageExpatAnnualNetSalary = averageExpatNetSalary.Value * 12.0m *
                                              (arePricesNominatedInUsd ? 1m / crossRateToUsd : 1m);
            
            var sumOfScores = citySalary.Items.Sum(i => i.Score ?? 1);
            
            var developerGrossSalaryP25 = citySalary.Items.Sum(i => i.P25 * (i.Score ?? 1)) / sumOfScores;
            var developerGrossSalaryP75 = citySalary.Items.Sum(i => i.P75 * (i.Score ?? 1)) / sumOfScores;
            
            var grossSalary = GetAnnualGrossSalary(cityName, salaryTypeId, manualSalary, crossRateToUsd, salaryMultiplicator, 
                averageExpatAnnualNetSalary, developerGrossSalaryP25, developerGrossSalaryP75);

            var hasFreeApartment = _citiesWithFreeApartment.Contains(cityName);

            var apartmentParameters = ReportApartmentParameters.Big;
            if (hasFreeApartment)
            {
                apartmentParameters |= ReportApartmentParameters.Free;
            }

            var cityItem = reportCityItemFactory.Create(new ReportCityApi(cityCosts.Name, cityCosts.Region, cityCosts.Country, countryCode),
                dataItems,
                taxDeductionItems, 
                new ReportCityDynamicParameters
                {
                    ApartmentParameters = apartmentParameters,
                    SelectedSalary = grossSalary,
                    AnnualGrossSalary = averageExpatAnnualNetSalary,
                    DeveloperGrossSalaryP25 = developerGrossSalaryP25,
                    DeveloperGrossSalaryP75 = developerGrossSalaryP75,
                    HouseholdMembers = new ReportHouseholdMembersParameters
                    {
                        TotalCount = userTaxProfile.UserFamily.FamilyMembersCount,
                        Toddlers = userTaxProfile.UserFamily.ToddlersCount,
                        Prescholars = userTaxProfile.UserFamily.PrescholarsCount,
                        Scholars = userTaxProfile.UserFamily.ScholarsCount
                    },
                    CurrencyCode = currencyCode,
                    CrossRateToUsd = crossRateToUsd,
                    ArePricesNominatedInUsd = arePricesNominatedInUsd
                });
    
            const decimal threshold = 20m;
            var averageGrossSalary = cityItem.SalaryData.AverageSalary.ValueGross.Value;
            var allCosts = cityItem.CostsWithRent.ValueNet.Value;
            
            if (averageGrossSalary / allCosts > threshold || averageGrossSalary / allCosts < 1m/threshold)
            {
                differErrors.Add($"Salary and costs differ too much for {cityName}, AverageSalary Gross = { averageGrossSalary }, All costs = { allCosts }");
            }
            reportCityExtendedApis.Add(cityItem);
        }

        if (differErrors.Any())
        {
            throw new Exception(string.Join(Environment.NewLine, differErrors));
        }
        
        return reportCityExtendedApis.Order(sortingFunction.SortingComparer(sortingDirection)).ToArray();
    }

    private static decimal GetAnnualGrossSalary(string cityName, int salaryTypeId, decimal? manualSalary, decimal crossRateToUsd, decimal? salaryMultiplicator,
        decimal? expatNetSalary, decimal developerGrossSalaryP25, decimal developerGrossSalaryP75)
    {
        decimal grossSalary;
        switch (salaryTypeId)
        {
            case 1: grossSalary = 0.0m;
                break;
            case 2:
                if (!expatNetSalary.HasValue || expatNetSalary.Value == 0.0m)
                {
                    throw new Exception($"Expat net salary not specified for {cityName}");
                }

                grossSalary = expatNetSalary.Value * (salaryMultiplicator ?? 1.0m);
                break;
            case 3 when manualSalary.HasValue:
                grossSalary = manualSalary.Value / crossRateToUsd;
                break;
            case 3 when !manualSalary.HasValue:
                throw new ArgumentException("Manual salary not specified", nameof(manualSalary));
                
            case 4:
                grossSalary = developerGrossSalaryP25 * (salaryMultiplicator ?? 1.0m);
                break;
                
            case 5:
                grossSalary = developerGrossSalaryP75 * (salaryMultiplicator ?? 1.0m);
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(salaryTypeId));
        }

        return grossSalary;
    }
}