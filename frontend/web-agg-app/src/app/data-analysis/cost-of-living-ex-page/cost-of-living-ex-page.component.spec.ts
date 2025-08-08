import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CostOfLivingExPageComponent } from './cost-of-living-ex-page.component';

describe('CostOfLivingExPageComponent', () => {
  let component: CostOfLivingExPageComponent;
  let fixture: ComponentFixture<CostOfLivingExPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CostOfLivingExPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CostOfLivingExPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
