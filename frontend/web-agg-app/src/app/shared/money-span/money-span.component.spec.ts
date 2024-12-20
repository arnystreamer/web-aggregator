import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoneySpanComponent } from './money-span.component';

describe('MoneySpanComponent', () => {
  let component: MoneySpanComponent;
  let fixture: ComponentFixture<MoneySpanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MoneySpanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MoneySpanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
