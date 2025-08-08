import { formatNumber } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ProfitTaxableApi } from '../../models/profit-taxable-api.model';
import { MoneySpanComponent } from '../money-span/money-span.component';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'wa-profit-taxable',
  imports: [
    MatIconModule,
    MoneySpanComponent,
    MatTooltipModule
  ],
  templateUrl: './profit-taxable.component.html',
  styleUrl: './profit-taxable.component.scss'
})
export class ProfitTaxableComponent {
  @Input() profit: ProfitTaxableApi | undefined;
  @Input() currencyCode: string | undefined;

  public getTooltipText() : string
  {
    if (!this.profit)
    {
      return 'No data';
    }

    let taxData = '';
    for (let i = 0; i < this.profit.taxBits.length; i++)
    {
      var currentTaxResult = this.profit.taxBits[i];
      taxData += currentTaxResult.name + ': ' + formatNumber(currentTaxResult.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + '\n';
    }

    return formatNumber(this.profit.valueGross.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') +
      ' (' + formatNumber(this.profit.valueGross.valueInUsd, 'en-GB', '1.0-0') + ' USD) gross \n'+
      taxData +
      formatNumber(this.profit.valueNet.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') +
      ' (' + formatNumber(this.profit.valueNet.valueInUsd, 'en-GB', '1.0-0') + ' USD) net';
  }
}
