import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { DictionaryDataItem } from '../../models/dictionary-data-item.model';
import { CityAggregated } from '../../models/city-aggregated.model';
import { City } from '../../models/city.model';
import { CityDataItemPopulated } from '../../models/city-data-item-populated.model';
import { CommonModule } from '@angular/common';
import { DiffSpanComponent } from '../../shared/diff-span/diff-span.component';
import { CitySalary } from '../../models/city-salary.model';
import { RegionTax } from '../../models/region-tax.model';
import { TaxLevel } from '../../models/tax-level.model';

@Component({
  selector: 'wa-cost-of-living-page',
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    RouterModule,
    DiffSpanComponent
  ],
  templateUrl: './cost-of-living-page.component.html',
  styleUrl: './cost-of-living-page.component.scss'
})
export class CostOfLivingPageComponent implements OnInit {

  public cities: CityAggregated[] = [];
  public dictionaryItems: DictionaryDataItem[] = [];
  public salaries: CitySalary[] = [];
  public regionTaxes: RegionTax[] = [];

  public overridenSalary?: number;
  public overridenSalaryMultiplicator?: number = 1.25;

  constructor(private route: ActivatedRoute)
  {

  }

  ngOnInit(): void {
    this.route.data.subscribe(({ cities, dictionary, salaries, regionTaxes }) => {
      const rawCities: City[] = cities.items;
      this.dictionaryItems.push(...dictionary.items);
      this.salaries.push(...salaries.items);
      this.regionTaxes.push(...regionTaxes.items);

      for(let rawCity of rawCities)
      {
        let dataItems: CityDataItemPopulated[] = rawCity.dataItems.filter(di => di.value).map(di => {
          return { dictionaryItem: this.dictionaryItems.filter(v => v.id == di.dictionaryId)[0], ...di }
        });

        let salaryItem: CitySalary = this.salaries.filter(s => s.city == rawCity.name)[0];
        let applicableRegionTaxes: RegionTax[] = this.regionTaxes.filter(s => s.region == rawCity.name || (s.region == null && s.country == rawCity.country));

        this.cities.push(
        {
          id: rawCity.id,
          name: rawCity.name,
          region: rawCity.region,
          country: rawCity.country,
          dataItems: dataItems.filter(i => i.dictionaryItem.key! > 30000),
          personalAll: this.getPersonalAll(dataItems),
          personalWithoutChildcare: this.getPersonalWithoutChildcare(dataItems),
          personalWithMortgageAndChildcare: this.getPersonalWithoutRent(dataItems) + this.getMortgagePayment(dataItems),
          salaries: salaryItem,
          applicableTaxes: applicableRegionTaxes,

          averageExpatSalaryNet: this.getSalary(dataItems),
          apartmentFirstPayment: this.getApartmentsPrice(dataItems) * 0.2,
          mortgageMonthlyPayment: this.getMortgagePayment(dataItems),
          averageApartmentsPrice: this.getApartmentsPrice(dataItems),
          averageRentPrice: this.getRentPrice(dataItems),

          averageExpatSalaryGrossYearly: this.unapplyTaxes(this.getSalary(dataItems) * 12, applicableRegionTaxes),
          sustainableSalaryNet: this.getPersonalWithoutChildcare(dataItems) + 1500,
          sustainableSalaryGross: this.unapplyTaxes(this.getPersonalWithoutChildcare(dataItems) + 1500, applicableRegionTaxes),
          p75SalaryNet: this.applyTaxes(salaryItem.p75!, applicableRegionTaxes) / 12,
          millionaireTerm: 1000000 / (this.getSalary(dataItems) - this.getPersonalAll(dataItems)),
          apartmentFirstPaymentTerm: this.getApartmentsPrice(dataItems) * 0.2 / (this.getSalary(dataItems) - this.getPersonalAll(dataItems))
        });
      }
    });
  }

  getSalary(dataItems: CityDataItemPopulated[])
  {
    if (this.overridenSalary)
      return this.overridenSalary;

    return dataItems.filter(i => i.dictionaryItem.key == 30301)[0].decimalValue! * (this.overridenSalaryMultiplicator || 1.0);
  }

  getApartmentsPrice(dataItems: CityDataItemPopulated[])
  {
    return dataItems
      .filter(di => di.dictionaryItem.key == 30202 &&
        di.decimalValue != null)[0].decimalValue! * 80;
  }

  getRentPrice(dataItems: CityDataItemPopulated[])
  {
    return dataItems
      .filter(di => di.dictionaryItem.key == 30104 &&
        di.decimalValue != null)[0].decimalValue!;
  }

  getInterestYearlyPercentage(dataItems: CityDataItemPopulated[])
  {
    return dataItems
      .filter(di => di.dictionaryItem.key == 30302 &&
        di.decimalValue != null)[0].decimalValue!;
  }

  getPersonalAll(dataItems: CityDataItemPopulated[]): number {
    return dataItems
      .filter(di => di.dictionaryItem.key != null &&
        di.dictionaryItem.key >= 10000 && di.dictionaryItem.key < 20000 &&
        di.decimalValue != null)
      .map(di => di.decimalValue)
      .reduce((prevSum, item) => prevSum! + item! , 0)!;
  }

  getPersonalWithoutRent(dataItems: CityDataItemPopulated[]): number {
    return dataItems
      .filter(di => di.dictionaryItem.key != null &&
        di.dictionaryItem.key >= 10000 && di.dictionaryItem.key < 20000 &&
        di.dictionaryItem.key != 11101 &&
        di.decimalValue != null)
      .map(di => di.decimalValue)
      .reduce((prevSum, item) => prevSum! + item! , 0)!;
  }

  getMortgagePayment(dataItems: CityDataItemPopulated[])
  {
    return this.getMortgageMonthlyPayment(
      this.getInterestYearlyPercentage(dataItems),
      30,
      this.getApartmentsPrice(dataItems) * 0.8)
  }

  getPersonalWithMortgageAndChildcare(dataItems: CityDataItemPopulated[]): number {
    return dataItems
      .filter(di => di.dictionaryItem.key != null &&
        di.dictionaryItem.key >= 10000 && di.dictionaryItem.key < 20000 &&
        di.decimalValue != null)
      .map(di => di.decimalValue)
      .reduce((prevSum, item) => prevSum! + item! , 0)!;
  }

  getPersonalWithoutChildcare(dataItems: CityDataItemPopulated[]): number {
    return dataItems
    .filter(di => di.dictionaryItem.key != null &&
      di.dictionaryItem.key >= 10000 && di.dictionaryItem.key < 20000 &&
      di.dictionaryItem.key != 11201 && di.dictionaryItem.key != 11202 &&
      di.decimalValue != null)
    .map(di => di.decimalValue)
    .reduce((prevSum, item) => prevSum! + item! , 0)!;
  }

  getMortgageMonthlyPayment(interestYearlyPercentage: number, years: number, debtAmount: number)
  {
    const r = interestYearlyPercentage / 100 / 12; // monthly interest rate
    const n = years * 12; //number of monthly payments

    return r * debtAmount * Math.pow(1 + r, n) / (Math.pow(1 + r, n) - 1);
  }

  applyTaxes(salary: number, taxes: RegionTax[]): number
  {
    let deduction = 0.0;
    for(let i = 0; i < taxes.length; i++) {
      const currentTax = taxes[i];
      deduction += currentTax.fixed;
      deduction += salary * currentTax.fixedRate;

      let previousLevel: TaxLevel | undefined;
      for(let j = 0; j < currentTax.taxLevels.length; i++)
      {
        const currentLevel = currentTax.taxLevels[j];
        if (previousLevel)
        {
          deduction += (currentLevel.lowerCut - previousLevel.lowerCut) * previousLevel.rate;
        }

        previousLevel = currentLevel;
      }

      if (previousLevel)
      {
        deduction += (salary - previousLevel.lowerCut) * previousLevel.rate;
      }
    }

    return salary - deduction;
  }

  unapplyTaxes(salary: number, taxes: RegionTax[]): number
  {
    let min = salary;
    let max = salary * 3;

    let counter = 10;
    let guess;
    do
    {
      guess = (min + max) / 2;

      var resultedSalary = this.applyTaxes(guess, taxes);

      if (resultedSalary > salary)
      {
        max = guess;
      }

      if (resultedSalary < salary)
      {
        min = guess;
      }
    }
    while(--counter > 0)

    return guess!;
  }
}
