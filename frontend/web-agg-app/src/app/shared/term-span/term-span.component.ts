import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'wa-term-span',
  imports: [CommonModule],
  templateUrl: './term-span.component.html',
  styleUrl: './term-span.component.scss'
})
export class TermSpanComponent {
    @Input() termInMonths?: number | null;
    @Input() selected: boolean = false;
}
