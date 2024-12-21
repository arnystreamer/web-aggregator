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
import { SortingFunctionsHelper } from '../../services/helpers/sorting-functions-helper';
import { TermSpanComponent } from '../../shared/term-span/term-span.component';
import { MoneySpanComponent } from '../../shared/money-span/money-span.component';

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
    DiffSpanComponent,
    TermSpanComponent,
    MoneySpanComponent
  ],
  templateUrl: './cost-of-living-page.component.html',
  styleUrl: './cost-of-living-page.component.scss'
})
export class CostOfLivingPageComponent implements OnInit {

  public filterForm!: FormGroup;
  public searchForm!: FormGroup;

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

  public selectedSortFnName?: string;

  public sortings = [
    { groupName : 'Salaries', items: [
        { name: 'Salary value', value: SortingFunctionsHelper.chosenSalaryAbsoulteValue },

        { name: 'Sustainable net salary value', value: SortingFunctionsHelper.sustainableSalaryNetAbsolute },
        { name: 'Sustainable net salary proximity abs.', value: SortingFunctionsHelper.sustainableSalaryNetProximity },
        { name: 'Sustainable net salary proximity %', value: SortingFunctionsHelper.sustainableSalaryNetProximityRelative },

        { name: 'Sustainable gross salary value', value: SortingFunctionsHelper.sustainableSalaryGrossAbsolute },

        { name: 'Salary 30-yrs-millionaire net value', value: SortingFunctionsHelper.millionaire30YSalaryNetAbsolute },
        { name: 'Salary 30-yrs-millionaire net proximity abs.', value: SortingFunctionsHelper.millionaire30YSalaryNetProximity },
        { name: 'Salary 30-yrs-millionaire net proximity %', value: SortingFunctionsHelper.millionaire30YSalaryNetProximityRelative },

        { name: 'Salary 30-yrs-millionaire gross value', value: SortingFunctionsHelper.millionaire30YSalaryGrossAbsolute },
      ]
    },
    { groupName : 'Expenses', items: [
        { name: 'Expenses as pair value', value: SortingFunctionsHelper.expensesAsPairAbsolute },
        { name: 'Expenses as pair proximity abs.', value: SortingFunctionsHelper.expensesAsPairProximity },
        { name: 'Expenses as pair proximity %', value: SortingFunctionsHelper.expensesAsPairProximityRelative },

        { name: 'Expenses with child value', value: SortingFunctionsHelper.expensesWithChildAbsolute },
        { name: 'Expenses with child proximity abs.', value: SortingFunctionsHelper.expensesWithChildProximity },
        { name: 'Expenses with child proximity %', value: SortingFunctionsHelper.expensesWithChildProximityRelative },

        { name: 'Expenses with child and mortgage value', value: SortingFunctionsHelper.expensesWithMortgageAndChildcareAbsolute },
        { name: 'Expenses with child and mortgage proximity abs.', value: SortingFunctionsHelper.expensesWithMortgageAndChildcareProximity },
        { name: 'Expenses with child and mortgage proximity %', value: SortingFunctionsHelper.expensesWithMortgageAndChildcareProximityRelative }
      ]
    },
    { groupName : 'Terms', items: [
        { name: 'Term to become millionaire', value: SortingFunctionsHelper.millionaireTerm },
        { name: 'Term to become millionaire without children', value: SortingFunctionsHelper.millionaireEasyTerm },
        { name: 'Term to buy ordinary car', value: SortingFunctionsHelper.buyCarTerm },
        { name: 'Term to accumulate 20% of apartment', value: SortingFunctionsHelper.apartmentFirstPaymentTerm }
      ]
    }
  ];

  public citiesWithFreeApartment: string[] = ['Moscow'];

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute)
  {

  }

  ngOnInit(): void {

    this.filterForm = this.formBuilder.group({
      selectedSalaryType: [this.salaryTypes[0]],
      manualSalary: [50000],
      salaryMultiplicator: [1],
      sorting: [this.sortings[0]],
      sortDirectionAscending: [true],
    });

    this.filterForm.controls['manualSalary'].disable();
    this.filterForm.controls['salaryMultiplicator'].disable();

    this.filterForm.controls['selectedSalaryType'].valueChanges.subscribe({next: v => {

      if (v.value == SalaryType.Expat || v.value == SalaryType.P25 || v.value == SalaryType.P75)
        this.filterForm.controls['salaryMultiplicator'].enable();
      else
        this.filterForm.controls['salaryMultiplicator'].disable();


      if (v.value == SalaryType.Manual)
        this.filterForm.controls['manualSalary'].enable();
      else
        this.filterForm.controls['manualSalary'].disable();
    }});

    this.searchForm = this.formBuilder.group({
      cityName: ['']
    });

    this.searchForm.controls['cityName'].valueChanges.subscribe({next: (v: String) => {

      var criteria: string = v ? v.trim().toLowerCase() : '';
      for(let city of this.cities)
      {
        city.isShown = (!criteria || city.name.trim().toLowerCase().includes(criteria));
      }

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

    if (!this.filterForm.controls['selectedSalaryType'].valid)
      return;

    this.searchForm.controls['cityName'].setValue('');

    var selectedSalaryType = this.filterForm.controls['selectedSalaryType'].value?.value;
    var manualSalary = this.filterForm.controls['manualSalary'].value;
    var salaryMultiplicator = this.filterForm.controls['salaryMultiplicator'].value;
    var sorting = this.filterForm.controls['sorting'].value;
    var sortDirectionAscending : boolean | undefined = this.filterForm.controls['sortDirectionAscending'].value;

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
          isShown: true,

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

    if (sorting || sortDirectionAscending != undefined)
    {
      this.selectedSortFnName = sorting.value.name;
      this.cities.sort(sorting.value(sortDirectionAscending));
    }
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

      for(let j = 0; j < currentTax.levels.length; j++)
      {
        const currentLevel = currentTax.levels[j];
        const nextLevelCut = j+1 < currentTax.levels.length ? currentTax.levels[j+1].lowerCut : 2147483647;

        deduction += Math.max((Math.min(nextLevelCut, salary) - currentLevel.lowerCut) * currentLevel.rate, 0);

        if (nextLevelCut >= salary)
          break;
      }
    }

    return salary - deduction;
  }

  unapplyTaxes(salary: number, taxes: RegionTax[]): number
  {
    let min = salary;
    let max = salary * 3;

    let counter = 8;
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
