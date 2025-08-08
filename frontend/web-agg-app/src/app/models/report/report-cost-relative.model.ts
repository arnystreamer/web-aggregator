import { ProfitTaxableApi } from "../profit-taxable-api.model";
import { ReportCost } from "./report-cost.model";

export interface ReportCostRelative extends ReportCost {
  baseProfit : ProfitTaxableApi;
  fractionValue : number;
  differenceValue : number;
}
