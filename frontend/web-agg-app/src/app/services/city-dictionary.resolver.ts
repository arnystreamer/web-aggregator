import { ResolveFn } from '@angular/router';
import { DictionaryDataItem } from '../models/dictionary-data-item.model';
import { CollectionApi } from '../models/collection-api.model';
import { inject } from '@angular/core';
import { CityDictionaryService } from './city-dictionary.service';

export const cityDictionaryResolver: ResolveFn<CollectionApi<DictionaryDataItem>> = (route, state) => {
    const cityDictionaryService = inject(CityDictionaryService);

    return cityDictionaryService.getAll(0, 10000);
};
