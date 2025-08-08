import { formatNumber } from "@angular/common";
import { MultiCurrencyValue } from "../../models/report/multi-currency-value.model";

export function currencyValuesToString(value: number, valueInUsd: number, currencyCode?: string, multiplier?: number): string
{
      return formatNumber(value * (multiplier ?? 1.0), 'en-GB', '1.0-0') + ' ' + (currencyCode ?? '???') +
      ' ($' + formatNumber(valueInUsd * (multiplier ?? 1.0), 'en-GB', '1.0-0') + ')';
}

export function multiCurrencyToString(multiCurrency: MultiCurrencyValue, currencyCode?: string, multiplier?: number): string
{
    return currencyValuesToString(multiCurrency.value, multiCurrency.valueInUsd, currencyCode, multiplier);
}
