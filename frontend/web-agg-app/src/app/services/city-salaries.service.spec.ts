import { TestBed } from '@angular/core/testing';

import { CitySalariesService } from './city-salaries.service';

describe('CitySalariesService', () => {
  let service: CitySalariesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CitySalariesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
