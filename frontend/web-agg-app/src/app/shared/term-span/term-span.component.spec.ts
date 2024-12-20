import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TermSpanComponent } from './term-span.component';

describe('TermSpanComponent', () => {
  let component: TermSpanComponent;
  let fixture: ComponentFixture<TermSpanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TermSpanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TermSpanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
