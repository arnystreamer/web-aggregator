import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfitDesirableComponent } from './profit-desirable.component';

describe('ProfitDesirableComponent', () => {
  let component: ProfitDesirableComponent;
  let fixture: ComponentFixture<ProfitDesirableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfitDesirableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfitDesirableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
