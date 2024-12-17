import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { citiesResolver } from './cities.resolver';

describe('citiesResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => citiesResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
