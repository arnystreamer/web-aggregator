import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CostChangesPageComponent } from './cost-changes-page.component';

describe('CostChangesPageComponent', () => {
  let component: CostChangesPageComponent;
  let fixture: ComponentFixture<CostChangesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CostChangesPageComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(CostChangesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
