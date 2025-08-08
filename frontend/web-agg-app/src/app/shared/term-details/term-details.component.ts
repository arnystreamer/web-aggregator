import { Component, Input } from '@angular/core';
import { ReportProfitTargetTerm } from '../../models/report-profit-target-term.model';
import { TermSpanComponent } from '../term-span/term-span.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { formatNumber } from '@angular/common';

@Component({
  selector: 'wa-term-details',
  imports: [TermSpanComponent, MatIconModule, MatTooltipModule],
  templateUrl: './term-details.component.html',
  styleUrl: './term-details.component.scss'
})
export class TermDetailsComponent {
  @Input() term: ReportProfitTargetTerm | undefined;
  @Input() currencyCode: string | undefined;

  public getTooltipText() : string
  {
    if (!this.term)
    {
      return 'No data';
    }

    this.term.income.valueNet

    return 'Monthly costs are ' + formatNumber(this.term.cost.valueNet.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + '\n' +
      'Annual net income is ' + formatNumber(this.term.income.valueNet.value, 'en-GB', '1.0-0') + ' or ' +
        formatNumber(this.term.income.valueNet.value / 12, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' monthly.\n' +
        'Surplus is ' + formatNumber(this.term.income.valueNet.value - this.term.cost.valueNet.value * 12, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' annually.\n' +
        'Target amount is ' + formatNumber(this.term.targetAmount.value, 'en-GB', '1.0-0') + ' ' + (this.currencyCode ?? '???') + ' will be ' +
        (this.term.termInYears
          ? 'reached in ' + formatNumber(this.term.termInYears, 'en-GB', '1.2-2') + ' years.'
          : 'never reached.');
  }
}
