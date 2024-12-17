import { ResolveFn } from '@angular/router';
import { CollectionApi } from '../models/collection-api.model';
import { CitySalary } from '../models/city-salary.model';
import { inject } from '@angular/core';
import { CitySalariesService } from './city-salaries.service';

export const citySalariesResolver: ResolveFn<CollectionApi<CitySalary>> = (route, state) => {
  const citySalariesService = inject(CitySalariesService);

  return citySalariesService.getAll(0, 10000);
};
