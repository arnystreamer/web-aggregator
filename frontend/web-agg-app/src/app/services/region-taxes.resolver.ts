import { ResolveFn } from '@angular/router';
import { RegionTax } from '../models/region-tax.model';
import { CollectionApi } from '../models/collection-api.model';
import { RegionTaxesService } from './region-taxes.service';
import { inject } from '@angular/core';

export const regionTaxesResolver: ResolveFn<CollectionApi<RegionTax>> = (route, state) => {
    const regionTaxesService = inject(RegionTaxesService);

    return regionTaxesService.getAll(0, 10000);
};
