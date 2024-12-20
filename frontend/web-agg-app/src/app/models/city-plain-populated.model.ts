import { CityDataItemPopulated } from "./city-data-item-populated.model";

export interface CityPlainPopulated {
  id: string;

  name: string;
  region: string;
  country: string;

  dataItems: CityDataItemPopulated[];
}
