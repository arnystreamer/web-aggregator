import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { regionTaxesResolver } from './region-taxes.resolver';

describe('regionTaxesResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => regionTaxesResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
