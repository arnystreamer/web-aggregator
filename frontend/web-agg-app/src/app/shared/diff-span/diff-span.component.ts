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

  @Input() baseSelected: boolean = false;
  @Input() relativeSelected: boolean = false;

  @Input() noColoring: boolean = false;

  value(): string
  {
    if (this.baseValue == undefined || this.relativeValue == undefined)
      return '';

    var value = this.baseValue - this.relativeValue;
    return formatNumber(value, 'en-GB', '1.0-0');
  }

  valuePercentage()
  {
    if (this.baseValue == undefined || this.relativeValue == undefined)
      return '';

    var valuePercentage = (this.baseValue - this.relativeValue) * 100.0 / this.relativeValue;
    return (valuePercentage > 10000 ? 'inf.' : formatNumber(valuePercentage, 'en-GB', '1.0-0') );
  }


  differenceClass(): string
  {
    if (this.baseValue == undefined || this.relativeValue == undefined)
      return '';

    if (this.baseValue > this.relativeValue * 1.1)
    {
      return 'value-bad';
    }

    if (this.baseValue * 1.1 < this.relativeValue)
    {
      return 'value-good';
    }

    return 'value-medium';
  }

}
