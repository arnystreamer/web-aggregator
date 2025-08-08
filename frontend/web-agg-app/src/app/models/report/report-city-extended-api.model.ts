import { ProfitTaxableApi } from "../profit-taxable-api.model";
import { ReportProfitTargetTerm } from "../report-profit-target-term.model";
import { MultiCurrencyValue } from "./multi-currency-value.model";
import { ProfitTaxableRelativeDesirableApi } from "./profit-taxable-relative-desirable-api.model";
import { ReportCityApi } from "./report-city-api.model";
import { ReportCostRelativeExtended } from "./report-cost-relative-extended.model";
import { SalaryData } from "./salary-data.model";

export interface ReportCityExtendedApi extends ReportCityApi {
  selectedSalary: ProfitTaxableApi;
  salaryData: SalaryData;
  currencyCode: string;
  hasFreeApartment: boolean;
  goingOutCosts: number;
  groceriesCosts: number;
  householdCosts: number;
  transportationCosts: number;
  utilitiesCosts: number;
  sportsAndLeisureCosts: number;
  clothingCosts: number;
  rentCosts: number;
  mortgageCosts: number;
  childcareCosts: number;
  vacationCosts: number;
  electronicsCosts: number;

  minimumCostsWithRent: ReportCostRelativeExtended;
  costsWithRent: ReportCostRelativeExtended;
  costsWithMortgage: ReportCostRelativeExtended;

  monthlySavingsWhileRenting: MultiCurrencyValue;
  monthlySavingsWhilePayingMortgage: MultiCurrencyValue;

  millionaireTerm: ReportProfitTargetTerm;
  mortgageDownPaymentTerm: ReportProfitTargetTerm;
  buyCarTerm: ReportProfitTargetTerm;

  salaryToEarn1MlnUsdIn30Yrs: ProfitTaxableRelativeDesirableApi;
  sustainableSalary: ProfitTaxableRelativeDesirableApi;
  bareMinimumSalary: ProfitTaxableRelativeDesirableApi;
}
