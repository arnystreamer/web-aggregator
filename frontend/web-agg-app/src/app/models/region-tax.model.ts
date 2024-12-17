import { TaxLevel } from "./tax-level.model";

export interface RegionTax {
  region?: string;
  country: string;
  fixed: number;
  fixedRate: number;
  taxLevels: TaxLevel[];
}
