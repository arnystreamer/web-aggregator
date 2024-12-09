import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CostOfLivingPageComponent } from './cost-of-living-page.component';

describe('CostOfLivingComponent', () => {
  let component: CostOfLivingPageComponent;
  let fixture: ComponentFixture<CostOfLivingPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CostOfLivingPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CostOfLivingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
