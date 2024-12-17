import { TestBed } from '@angular/core/testing';

import { CityDictionaryService } from './city-dictionary.service';

describe('CityAttributesService', () => {
  let service: CityDictionaryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CityDictionaryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
