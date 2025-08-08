import { ReportProfit } from "./report-profit.model";

export interface ReportProfitRelative extends ReportProfit {
  baseValueGross : number;
  fractionValue : number;
  differenceValue : number;
}
