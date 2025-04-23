import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'wa-money-span',
  imports: [ CommonModule ],
  templateUrl: './money-span.component.html',
  styleUrl: './money-span.component.scss'
})
export class MoneySpanComponent {
  @Input() value?: number | (() => number | undefined);
  @Input() selected: boolean = false;

  getValue() : number | undefined
  {
    if (this.value === undefined)
      return undefined;

    if (typeof this.value === "number")
      return this.value;

    if (typeof this.value === "function")
    {
      try
      {
        return this.value();
      }
      catch
      {
        return undefined;
      }
    }

    return undefined;
  }

}
