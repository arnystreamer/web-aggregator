import { MultiCurrencyValue } from "./multi-currency-value.model";

export interface ReportProfit {
  valueGross: MultiCurrencyValue;
  valueNet: MultiCurrencyValue;
}
