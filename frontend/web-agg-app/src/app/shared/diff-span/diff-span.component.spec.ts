import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DiffSpanComponent } from './diff-span.component';

describe('DiffSpanComponent', () => {
  let component: DiffSpanComponent;
  let fixture: ComponentFixture<DiffSpanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DiffSpanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DiffSpanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
