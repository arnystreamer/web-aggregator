import { formatNumber } from '@angular/common';
import { Component, computed, Input, Signal } from '@angular/core';
import { MoneySpanComponent } from '../money-span/money-span.component';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ProfitTaxableRelativeDesirableApi } from '../../models/report/profit-taxable-relative-desirable-api.model';
import { DiffSpanComponent } from '../diff-span/diff-span.component';
import { PresentationService } from '../../services/presentation.service';
import { multiCurrencyToString, currencyValuesToString } from '../../services/helpers/money-display-helper'

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
  @Input() grossSelected: boolean = false;
  @Input() diffSelected: boolean = false;
  @Input() relativeSelected: boolean = false;

  public isShowInUsd: Signal<boolean> = computed(() => this.presentationService.currencySignal() == 'USD')

  constructor(private presentationService: PresentationService)
  {

  }

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
    var savingsInUsdPerMonth = this.profit.annualSavings.valueInUsd / 12;
    var bareCostsPerMonth = this.profit.requestedAnnualIncome.value / 12 - savingsPerMonth;

    return 'Costs: ' + formatNumber(bareCostsPerMonth, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') +
      ' and savings ' + currencyValuesToString(savingsPerMonth, savingsInUsdPerMonth, this.currencyCode) + '\n' +
      'Essential amount is ' + formatNumber(this.profit.requestedAnnualIncome.value / 12, 'en-GB', '1.0-0') + ' monthly or ' +
      formatNumber(this.profit.requestedAnnualIncome.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' yearly \n' +
      '\nSalary calculations: \n' +
      multiCurrencyToString(this.profit.valueNet, this.currencyCode) + ' net \n'+
      taxData +
      multiCurrencyToString(this.profit.valueGross, this.currencyCode) + ' gross';
  }

  public getDiffTooltipText(): string
  {
    if (!this.profit)
    {
      return 'No data';
    }

    return 'Desirable salary is ' + multiCurrencyToString(this.profit.valueGross, this.currencyCode) + ' gross yearly\n' +
      'Fact is ' + formatNumber(this.profit.baseValueGross, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' gross';
  }
}
