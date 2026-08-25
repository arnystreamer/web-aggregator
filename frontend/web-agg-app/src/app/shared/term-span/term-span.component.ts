import { CommonModule } from '@angular/common';
import { Component, Input, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'wa-term-span',
  imports: [CommonModule],
  templateUrl: './term-span.component.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './term-span.component.scss'
})
export class TermSpanComponent {
    @Input() termInMonths?: number | null;
    @Input() selected: boolean = false;
}
