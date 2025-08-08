import { formatNumber } from '@angular/common';
import { Component, computed, Input, Signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MoneySpanComponent } from '../money-span/money-span.component'
import { DiffSpanComponent } from '../diff-span/diff-span.component';
import { ReportCostRelativeExtended } from '../../models/report/report-cost-relative-extended.model';
import { PresentationService } from '../../services/presentation.service';
import { multiCurrencyToString } from '../../services/helpers/money-display-helper'


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
  @Input() valueSelected: boolean = false;
  @Input() diffSelected: boolean = false;
  @Input() relativeSelected: boolean = false;

  public isShowInUsd: Signal<boolean> = computed(() => this.presentationService.currencySignal() == 'USD')

  constructor(private presentationService: PresentationService)
  {
    presentationService.currencySignal()
  }

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
      'Monthly costs are ' + multiCurrencyToString(this.cost.valueNet, this.currencyCode) + ' or ' +
      formatNumber(this.cost.valueNet.value * 12, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' yearly \n' +

      'Annual profit is ' + formatNumber(this.cost.baseProfit.valueNet.value, 'en-GB', '1.0-0') + ' or ' +
      multiCurrencyToString(this.cost.baseProfit.valueNet, this.currencyCode, 1/12) + ' monthly';
  }

  getDiffTooltipText(): string
  {
    if (!this.cost)
    {
      return 'No data';
    }

    return 'Monthly costs are ' + multiCurrencyToString(this.cost.valueNet, this.currencyCode) + '\n' +
      'Profit is ' + multiCurrencyToString(this.cost.baseProfit.valueNet, this.currencyCode, 1/12) + ' monthly or ' +
      formatNumber(this.cost.baseProfit.valueNet.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' yearly';
  }
}

