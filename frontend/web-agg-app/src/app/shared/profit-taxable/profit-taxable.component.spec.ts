import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfitTaxableComponent } from './profit-taxable.component';

describe('ProfitTaxableComponent', () => {
  let component: ProfitTaxableComponent;
  let fixture: ComponentFixture<ProfitTaxableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfitTaxableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfitTaxableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
