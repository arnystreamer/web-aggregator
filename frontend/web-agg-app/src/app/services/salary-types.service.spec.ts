import { TestBed } from '@angular/core/testing';

import { SalaryTypesService } from './salary-types.service';

describe('SalaryTypesService', () => {
  let service: SalaryTypesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SalaryTypesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
