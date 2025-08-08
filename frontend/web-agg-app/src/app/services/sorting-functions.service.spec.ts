import { TestBed } from '@angular/core/testing';

import { SortingFunctionsService } from './sorting-functions.service';

describe('SortingFunctionsService', () => {
  let service: SortingFunctionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SortingFunctionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
