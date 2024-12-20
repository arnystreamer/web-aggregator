import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'wa-money-span',
  imports: [ CommonModule ],
  templateUrl: './money-span.component.html',
  styleUrl: './money-span.component.scss'
})
export class MoneySpanComponent {
  @Input() value?: number;
  @Input() selected: boolean = false;
}
