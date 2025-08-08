import { ProfitTaxableApi } from "./profit-taxable-api.model";

export interface ProfitTaxableRelativeApi extends ProfitTaxableApi {
  baseValueGross : number;
  fractionValue: number;
  differenceValue: number;
}
