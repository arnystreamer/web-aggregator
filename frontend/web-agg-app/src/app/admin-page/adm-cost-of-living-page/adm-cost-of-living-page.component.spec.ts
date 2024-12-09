import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdmCostOfLivingPageComponent } from './adm-cost-of-living-page.component';

describe('AdmCostOfLivingComponent', () => {
  let component: AdmCostOfLivingPageComponent;
  let fixture: ComponentFixture<AdmCostOfLivingPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdmCostOfLivingPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdmCostOfLivingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
