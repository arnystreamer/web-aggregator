import { TestBed } from '@angular/core/testing';

import { RegionTaxesService } from './region-taxes.service';

describe('RegionTaxesService', () => {
  let service: RegionTaxesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RegionTaxesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
