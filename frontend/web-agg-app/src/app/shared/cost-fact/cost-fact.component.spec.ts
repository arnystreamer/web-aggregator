import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CostFactComponent } from './cost-fact.component';

describe('CostFactComponent', () => {
  let component: CostFactComponent;
  let fixture: ComponentFixture<CostFactComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CostFactComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CostFactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
