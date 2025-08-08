import { MultiCurrencyValue } from "./report/multi-currency-value.model";
import { TaxBitApi } from "./tax-bit-api.model";

export interface ProfitTaxableApi {
  valueGross: MultiCurrencyValue;
  valueNet: MultiCurrencyValue;
  totalDeductions: number;
  taxBits: TaxBitApi[];
}
