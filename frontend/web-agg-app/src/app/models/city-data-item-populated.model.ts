import { DictionaryDataItem } from "./dictionary-data-item.model";

export interface CityDataItemPopulated {
  key: string;
  dictionaryId: string;
  value: string;
  decimalValue: number | null;

  dictionaryItem : DictionaryDataItem;
}
