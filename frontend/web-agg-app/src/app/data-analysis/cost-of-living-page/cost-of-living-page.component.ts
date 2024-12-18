import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DictionaryDataItem } from '../../models/dictionary-data-item.model';
import { CityAggregated } from '../../models/city-aggregated.model';
import { City } from '../../models/city.model';
import { CityDataItemPopulated } from '../../models/city-data-item-populated.model';
import { CommonModule } from '@angular/common';
import { DiffSpanComponent } from '../../shared/diff-span/diff-span.component';
import { CitySalary } from '../../models/city-salary.model';
import { RegionTax } from '../../models/region-tax.model';
import { TaxLevel } from '../../models/tax-level.model';
import { SalaryType } from '../../models/salary-type.enum';

@Component({
  selector: 'wa-cost-of-living-page',
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    ReactiveFormsModule,
    MatInputModule,
    RouterModule,
    DiffSpanComponent
  ],
  templateUrl: './cost-of-living-page.component.html',
  styleUrl: './cost-of-living-page.component.scss'
})
export class CostOfLivingPageComponent implements OnInit {

  public form!: FormGroup;

  public rawCities: City[] = [];
  public dictionaryItems: DictionaryDataItem[] = [];
  public salaries: CitySalary[] = [];
  public regionTaxes: RegionTax[] = [];

  public cities: CityAggregated[] = [];

  public salaryTypes = [
    { name: 'Zero', value: SalaryType.Zero },
    { name: 'Expat salary', value: SalaryType.Expat },
    { name: 'Manual', value: SalaryType.Manual },
    { name: 'Developer bottom 25%', value: SalaryType.P25 },
    { name: 'Developer top 25%', value: SalaryType.P75 }
  ];

  public citiesWithFreeApartment: string[] = ['Moscow'];

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute)
  {

  }

  ngOnInit(): void {

    this.form = this.formBuilder.group({
      selectedSalaryType: [this.salaryTypes[0]],
      manualSalary: [50000],
      salaryMultiplicator: [1],
    });

    this.form.controls['manualSalary'].disable();
    this.form.controls['salaryMultiplicator'].disable();

    this.form.controls['selectedSalaryType'].valueChanges.subscribe({next: v => {

      if (v.value == SalaryType.Expat || v.value == SalaryType.P25 || v.value == SalaryType.P75)
        this.form.controls['salaryMultiplicator'].enable();
      else
        this.form.controls['salaryMultiplicator'].disable();


      if (v.value == SalaryType.Manual)
        this.form.controls['manualSalary'].enable();
      else
        this.form.controls['manualSalary'].disable();
    }});

    this.route.data.subscribe(({ cities, dictionary, salaries, regionTaxes }) => {
      this.rawCities.push(...cities.items);
      this.dictionaryItems.push(...dictionary.items);
      this.salaries.push(...salaries.items);
      this.regionTaxes.push(...regionTaxes.items);
    });
  }

  rebuildTable()
  {
    this.cities = [];

    if (!this.form.controls['selectedSalaryType'].valid)
      return;

    var selectedSalaryType = this.form.controls['selectedSalaryType'].value?.value;
    var manualSalary = this.form.controls['manualSalary'].value;
    var salaryMultiplicator = this.form.controls['salaryMultiplicator'].value;

    for(let rawCity of this.rawCities)
    {
        let dataItems: CityDataItemPopulated[] = rawCity.dataItems.filter(di => di.value).map(di => {
          return { dictionaryItem: this.dictionaryItems.filter(v => v.id == di.dictionaryId)[0], ...di }
        });

        let salaryItem: CitySalary = this.salaries.filter(s => s.city == rawCity.name)[0];
        let applicableRegionTaxes: RegionTax[] = this.regionTaxes.filter(s => s.region == rawCity.name || (s.region == null && s.country == rawCity.country));

        let cityAggregated: CityAggregated =
        {
          id: rawCity.id,
          name: rawCity.name,
          region: rawCity.region,
          country: rawCity.country,
          dataItems: dataItems.filter(i => i.dictionaryItem.key! > 30000),
          hasFreeApartment: this.hasFreeApartment(rawCity),
          personalAll: !this.hasFreeApartment(rawCity) ? this.getPersonalAll(dataItems) : this.getPersonalAllWithoutRent(dataItems),
          personalWithoutChildcare: !this.hasFreeApartment(rawCity) ? this.getPersonalWithoutChildcare(dataItems) : this.getPersonalWithoutChildcareWithoutRent(dataItems),
          personalWithMortgageAndChildcare: this.getPersonalAllWithoutRent(dataItems) + this.getMortgagePayment(dataItems),
          salaries: salaryItem,
          applicableTaxes: applicableRegionTaxes,

          averageExpatSalaryNet: this.getExpatSalary(dataItems),
          apartmentFirstPayment: this.getApartmentsPrice(dataItems) * 0.2,
          mortgageMonthlyPayment: this.getMortgagePayment(dataItems),
          averageApartmentsPrice: this.getApartmentsPrice(dataItems),
          averageRentPrice: this.getRentPrice(dataItems),
          averageOrdinalCarPrice: this.getAverageOrdinalCarPrice(dataItems)
        };

        cityAggregated.averageExpatSalaryGrossYearly = this.unapplyTaxes(cityAggregated.averageExpatSalaryNet * 12, applicableRegionTaxes);
        cityAggregated.p25SalaryNet = this.applyTaxes(salaryItem.p25!, applicableRegionTaxes) / 12;
        cityAggregated.p75SalaryNet = this.applyTaxes(salaryItem.p75!, applicableRegionTaxes) / 12;

        cityAggregated.sustainableSalaryNet = cityAggregated.personalWithoutChildcare + 1500;
        cityAggregated.sustainableSalaryGross = this.unapplyTaxes(cityAggregated.sustainableSalaryNet * 12, applicableRegionTaxes);

        cityAggregated.millionaire30YSalaryNet = cityAggregated.personalAll + 2770;
        cityAggregated.millionaire30YSalaryGross = this.unapplyTaxes(cityAggregated.millionaire30YSalaryNet * 12, applicableRegionTaxes);

        cityAggregated.chosenSalaryNetMonthly = this.getSalary(selectedSalaryType, manualSalary, salaryMultiplicator, cityAggregated)!;

        cityAggregated.millionaireTerm = 1000000 / (cityAggregated.chosenSalaryNetMonthly - cityAggregated.personalAll);
        cityAggregated.millionaireEasyTerm = 1000000 / (cityAggregated.chosenSalaryNetMonthly - cityAggregated.personalWithoutChildcare);
        cityAggregated.buyCarTerm = cityAggregated.averageOrdinalCarPrice / (cityAggregated.chosenSalaryNetMonthly - cityAggregated.personalWithoutChildcare);
        cityAggregated.apartmentFirstPaymentTerm = cityAggregated.apartmentFirstPayment / (cityAggregated.chosenSalaryNetMonthly - cityAggregated.personalAll);

        this.cities.push(cityAggregated);
    }

    var sustainableSalaryNetProximityAsc = (a: CityAggregated, b: CityAggregated) =>
      a.sustainableSalaryNet! - a.chosenSalaryNetMonthly! - (b.sustainableSalaryNet! - b.chosenSalaryNetMonthly!);

    this.cities.sort(sustainableSalaryNetProximityAsc);
  }


  hasFreeApartment(city: City): boolean
  {
    return this.citiesWithFreeApartment.indexOf(city.name) > -1;
  }

  getSalary(salaryType: SalaryType, manualGrossSalary: number, multiplicator: number, city: CityAggregated): number
  {
    switch(salaryType)
    {
      case SalaryType.Zero:
        return 0;
      case SalaryType.Expat:
        return city.averageExpatSalaryNet * (multiplicator || 1);
      case SalaryType.Manual:
        return this.applyTaxes(manualGrossSalary, city.applicableTaxes) / 12.0;
      case SalaryType.P25:
        return city.p25SalaryNet! * (multiplicator || 1);
      case SalaryType.P75:
        return city.p75SalaryNet! * (multiplicator || 1);
    }
  }

  getExpatSalary(dataItems: CityDataItemPopulated[])
  {
    return dataItems.filter(i => i.dictionaryItem.key == 30301)[0].decimalValue!;
  }

  getAverageOrdinalCarPrice(dataItems: CityDataItemPopulated[])
  {
    return dataItems
      .filter(di => di.dictionaryItem.key == 20307 &&
        di.decimalValue != null)[0].decimalValue!;
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

  getPersonalAllWithoutRent(dataItems: CityDataItemPopulated[]): number {
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

  getPersonalWithoutChildcareWithoutRent(dataItems: CityDataItemPopulated[]): number {
    return dataItems
    .filter(di => di.dictionaryItem.key != null &&
      di.dictionaryItem.key >= 10000 && di.dictionaryItem.key < 20000 &&
      di.dictionaryItem.key != 11201 && di.dictionaryItem.key != 11202 &&
      di.dictionaryItem.key != 11101 &&
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

      let previousLevel: TaxLevel | undefined = currentTax.levels.length > 0 ? currentTax.levels[0] : undefined;
      for(let j = 1; j < currentTax.levels.length; j++)
      {
        previousLevel = currentTax.levels[j-1];
        const currentLevel = currentTax.levels[j];

        if (currentLevel.lowerCut > salary)
        {
          break;
        }

        if (previousLevel)
        {
          deduction += (currentLevel.lowerCut - previousLevel.lowerCut) * previousLevel.rate;
        }
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
    let max = salary * 2.2;

    let counter = 7;
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
