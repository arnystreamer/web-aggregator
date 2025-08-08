import { CostBitApi } from "../cost-bit-api.model";
import { ReportCost } from "./report-cost.model";

export interface ReportCostExtended extends ReportCost {
  costBits: CostBitApi[];
}
