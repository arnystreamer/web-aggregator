import { formatNumber } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'wa-diff-span',
  imports: [],
  templateUrl: './diff-span.component.html',
  styleUrl: './diff-span.component.scss'
})
export class DiffSpanComponent {
  @Input() baseValue?: number;
  @Input() relativeValue?: number;

  @Input() noColoring: boolean = false;

  text(): string
  {
    if (!this.baseValue || !this.relativeValue)
      return '';

    return formatNumber((this.baseValue - this.relativeValue) * 100 / this.relativeValue, 'en-GB', '1.0-0')+'%';
  }


  differenceClass(): string
  {
    if (!this.baseValue || !this.relativeValue)
      return '';

    if (this.baseValue > this.relativeValue * 1.2)
    {
      return 'value-bad';
    }

    if (this.baseValue * 1.2 < this.relativeValue)
    {
      return 'value-good';
    }

    return 'value-medium';
  }

}
