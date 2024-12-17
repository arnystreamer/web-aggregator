import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CitiesPlainPageComponent } from './cities-plain-page.component';

describe('CitiesPlainPageComponent', () => {
  let component: CitiesPlainPageComponent;
  let fixture: ComponentFixture<CitiesPlainPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CitiesPlainPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CitiesPlainPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
