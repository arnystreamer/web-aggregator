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

  public overridenSalary?: number;
  public overridenSalaryMultiplicator?: number = 1.25;

  constructor(private route: ActivatedRoute)
  {

  }

  ngOnInit(): void {
    this.route.data.subscribe(({ cities, dictionary }) => {
      const rawCities: City[] = cities.items;
      this.dictionaryItems.push(...dictionary.items);

      for(let rawCity of rawCities)
      {
        let dataItems: CityDataItemPopulated[] = rawCity.dataItems.filter(di => di.value).map(di => {
          return { dictionaryItem: this.dictionaryItems.filter(v => v.id == di.dictionaryId)[0], ...di }
        });

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

          averageExpatSalary: this.getSalary(dataItems),
          apartmentFirstPayment: this.getApartmentsPrice(dataItems) * 0.2,
          mortgageMonthlyPayment: this.getMortgagePayment(dataItems),
          averageApartmentsPrice: this.getApartmentsPrice(dataItems),
          averageRentPrice: this.getRentPrice(dataItems),

          sustainableSalary: this.getPersonalWithoutChildcare(dataItems) + 1500,
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
}
