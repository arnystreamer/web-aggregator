import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { cityDictionaryResolver } from './city-dictionary.resolver';

describe('cityDictionaryResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => cityDictionaryResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
