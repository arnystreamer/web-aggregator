import { ProfitTaxableApi } from "./profit-taxable-api.model";
import { MultiCurrencyValue } from "./report/multi-currency-value.model";
import { ReportCostExtended } from "./report/report-cost-extended.model";

export interface ReportProfitTargetTerm {
  income: ProfitTaxableApi;
  cost: ReportCostExtended;
  targetAmount: MultiCurrencyValue;
  termInMonths: number | null;
  termInYears: number | null;
}
