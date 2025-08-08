import { formatNumber } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MoneySpanComponent } from '../money-span/money-span.component';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ProfitTaxableRelativeDesirableApi } from '../../models/report/profit-taxable-relative-desirable-api.model';
import { DiffSpanComponent } from '../diff-span/diff-span.component';

@Component({
  selector: 'wa-profit-desirable',
  imports: [
    MatIconModule,
    MoneySpanComponent,
    MatTooltipModule,
    DiffSpanComponent
  ],
  templateUrl: './profit-desirable.component.html',
  styleUrl: './profit-desirable.component.scss'
})
export class ProfitDesirableComponent {
  @Input() profit: ProfitTaxableRelativeDesirableApi | undefined;
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

    var savingsPerMonth = this.profit.annualSavings.value / 12;
    var savingsInUsdPerMonth = this.profit.annualSavings.value / 12;
    var bareCostsPerMonth = this.profit.requestedAnnualIncome.value / 12 - savingsPerMonth;

    return 'Costs: ' + formatNumber(bareCostsPerMonth, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') +
      ' and savings ' + formatNumber(savingsPerMonth, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') +
      ' (' + formatNumber(savingsInUsdPerMonth, 'en-GB', '1.0-0') + ' USD) \n' +
      'Essential amount is ' + formatNumber(this.profit.requestedAnnualIncome.value / 12, 'en-GB', '1.0-0') + ' monthly or ' +
      formatNumber(this.profit.requestedAnnualIncome.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' yearly \n' +
      '\nSalary calculations: \n' +
      formatNumber(this.profit.valueNet.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') +
      ' (' + formatNumber(this.profit.valueNet.valueInUsd, 'en-GB', '1.0-0') + ' USD) net \n'+
      taxData +
      formatNumber(this.profit.valueGross.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') +
      ' (' + formatNumber(this.profit.valueGross.valueInUsd, 'en-GB', '1.0-0') + ' USD) gross';
  }

  public getDiffTooltipText(): string
  {
    if (!this.profit)
    {
      return 'No data';
    }

    return 'Desirable salary is ' + formatNumber(this.profit.valueGross.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' gross yearly\n' +
      'Fact is ' + formatNumber(this.profit.baseValueGross, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' gross';
  }
}
