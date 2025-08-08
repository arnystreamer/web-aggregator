import { formatNumber } from '@angular/common';
import { Component, computed, Input, Signal } from '@angular/core';
import { ProfitTaxableApi } from '../../models/profit-taxable-api.model';
import { MoneySpanComponent } from '../money-span/money-span.component';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { PresentationService } from '../../services/presentation.service';
import { multiCurrencyToString } from '../../services/helpers/money-display-helper'

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
  @Input() netSelected: boolean = false;
  @Input() grossSelected: boolean = false;

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

    return multiCurrencyToString(this.profit.valueGross, this.currencyCode) + ' gross\n'+
      taxData +
      multiCurrencyToString(this.profit.valueNet, this.currencyCode) + ' USD) net';
  }
}
