import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { citySalariesResolver } from './city-salaries.resolver';

describe('citySalariesResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => citySalariesResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
