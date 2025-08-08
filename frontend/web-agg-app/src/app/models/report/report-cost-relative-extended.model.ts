import { ProfitTaxableApi } from "../profit-taxable-api.model";
import { ReportCostExtended } from "./report-cost-extended.model";

export interface ReportCostRelativeExtended extends ReportCostExtended {
    baseProfit : ProfitTaxableApi;
    fractionValue : number;
    differenceValue : number;
}
