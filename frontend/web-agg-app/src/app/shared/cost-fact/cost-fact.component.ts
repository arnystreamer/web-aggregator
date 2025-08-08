import { formatNumber } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MoneySpanComponent } from '../money-span/money-span.component'
import { DiffSpanComponent } from '../diff-span/diff-span.component';
import { ReportCostRelativeExtended } from '../../models/report/report-cost-relative-extended.model';


@Component({
  selector: 'wa-cost-fact',
  imports: [
    MatIconModule,
    MatTooltipModule,
    MoneySpanComponent,
    DiffSpanComponent
  ],
  templateUrl: './cost-fact.component.html',
  styleUrl: './cost-fact.component.scss'
})
export class CostFactComponent {
  @Input() cost: ReportCostRelativeExtended | undefined;
  @Input() currencyCode: string | undefined;

  getTooltipText(): string
  {
    if (!this.cost)
    {
      return 'No data';
    }

    let costData = '';
    for (let i = 0; i < this.cost.costBits.length; i++)
    {
      var currentCostBit = this.cost.costBits[i];
      costData += currentCostBit.name + ': ' + formatNumber(currentCostBit.value, 'en-GB', '1.0-0') + '\n';
    }

    return costData +
      'Monthly costs are ' + formatNumber(this.cost.valueNet.value, 'en-GB', '1.0-0') + ' or ' +
      formatNumber(this.cost.valueNet.value * 12, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + '\n' +
      'Annual income is ' + formatNumber(this.cost.baseProfit.valueNet.value, 'en-GB', '1.0-0') + ' or ' +
        formatNumber(this.cost.baseProfit.valueNet.value / 12, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' monthly';
  }

  getDiffTooltipText(): string
  {
    if (!this.cost)
    {
      return 'No data';
    }

    return 'Monthly costs are ' + formatNumber(this.cost.valueNet.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') +
      ' (' + formatNumber(this.cost.valueNet.valueInUsd, 'en-GB', '1.0-0') + ' USD) \n' +
      'Profit is ' + formatNumber(this.cost.baseProfit.valueNet.value / 12, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' monthly or ' +
      formatNumber(this.cost.baseProfit.valueNet.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' yearly';
  }
}
