import { CityDataItem } from "./city-data-item.model";

export interface City {
  id: string;

  name: string;
  region: string;
  country: string;

  dataItems: CityDataItem[];
}
