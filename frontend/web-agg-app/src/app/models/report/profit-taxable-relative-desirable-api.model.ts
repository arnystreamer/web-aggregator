import { CostBitApi } from "../cost-bit-api.model";
import { ProfitTaxableRelativeApi } from "../profit-taxable-relative-api.model";
import { MultiCurrencyValue } from "./multi-currency-value.model";

export interface ProfitTaxableRelativeDesirableApi extends ProfitTaxableRelativeApi {
  requestedAnnualIncome: MultiCurrencyValue;
  costBits: CostBitApi[];
  annualSavings: MultiCurrencyValue;
}
