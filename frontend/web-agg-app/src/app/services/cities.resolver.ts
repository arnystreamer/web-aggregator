import { ResolveFn } from '@angular/router';
import { CollectionApi } from '../models/collection-api.model';
import { City } from '../models/city.model';
import { CitiesService } from './cities.service';
import { inject } from '@angular/core';

export const citiesResolver: ResolveFn<CollectionApi<City>> = (route, state) => {
  const citiesService = inject(CitiesService);

  return citiesService.getAll(0, 10000);
};
