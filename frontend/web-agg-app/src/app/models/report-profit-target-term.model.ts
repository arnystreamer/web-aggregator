import { ProfitTaxableApi } from "./profit-taxable-api.model";
import { MultiCurrencyValue } from "./report/multi-currency-value.model";

export interface ReportProfitTargetTerm {
  income: ProfitTaxableApi;
  targetAmount: MultiCurrencyValue;
  termInMonths: number | null;
  termInYears: number | null;
}
