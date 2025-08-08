import { CommonModule } from '@angular/common';
import { Component, computed, OnInit, signal, Signal, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ReportService } from '../../services/report.service';
import { SalaryTypesService } from '../../services/salary-types.service';
import { SortingFunctionsService } from '../../services/sorting-functions.service';
import { SalaryTypeApi } from '../../models/report/salary-type-api.model';
import { SortingFunctionApi } from '../../models/report/sorting-function-api.model';

import { ReportCityExtendedApi } from '../../models/report/report-city-extended-api.model';
import { CollectionApi } from '../../models/collection-api.model';
import { ProfitTaxableComponent } from '../../shared/profit-taxable/profit-taxable.component';
import { CostFactComponent } from "../../shared/cost-fact/cost-fact.component";
import { ProfitDesirableComponent } from "../../shared/profit-desirable/profit-desirable.component";
import { TermDetailsComponent } from "../../shared/term-details/term-details.component";
import { PresentationService } from '../../services/presentation.service';

@Component({
  selector: 'wa-cost-of-living-ex-page',
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    ReactiveFormsModule,
    MatInputModule,
    MatTooltipModule,
    RouterModule,
    ProfitTaxableComponent,
    CostFactComponent,
    ProfitDesirableComponent,
    TermDetailsComponent
],
  templateUrl: './cost-of-living-ex-page.component.html',
  styleUrl: './cost-of-living-ex-page.component.scss'
})
export class CostOfLivingExPageComponent implements OnInit {

  public filterForm?: FormGroup;
  public adjustForm?: FormGroup;

  public salaryTypes: SalaryTypeApi[] = [];
  public sortingFunctions: SortingFunctionApi[] = [];

  public selectedSalaryType?: SalaryTypeApi;
  public selectedSortingFunction?: SortingFunctionApi;
  public selectedCurrencySignal?: WritableSignal<string | undefined> = signal(undefined);

  public reportSalaryType: WritableSignal<SalaryTypeApi | undefined> = signal(undefined);
  public reportSortingFunction: WritableSignal<SortingFunctionApi | undefined> = signal(undefined);
  public reportResultItems: WritableSignal<ReportCityExtendedApi[] | undefined> = signal(undefined)
  public reportResultItemsFilterString: WritableSignal<string> = signal('');
  public reportResultFilteredItems: Signal<ReportCityExtendedApi[] | undefined> = computed(
    () =>
    {
      var filterString = this.reportResultItemsFilterString();
      if (!filterString || filterString == null)
          return this.reportResultItems();

      var filterStringFormatted = filterString.toLowerCase();

      return this.reportResultItems()?.filter(
        v => (v.name + ' ' + v.region + ' ' + v.country).toLowerCase().indexOf(filterStringFormatted) > -1);
});

  public reportSortingFunctionName: Signal<string | undefined> = computed(() => this.reportSortingFunction()?.functionName);

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private reportService: ReportService,
    private salaryTypesService: SalaryTypesService, private sortingFunctionsService : SortingFunctionsService,
    private presentationService: PresentationService) {

    this.adjustForm = this.formBuilder.group({
      currency: ['local'],
      city: ['']
    });

    this.adjustForm.controls['currency'].valueChanges.subscribe(v => this.presentationService.setCurrency(v));
    this.adjustForm.controls['city'].valueChanges.subscribe(v => { this.reportResultItemsFilterString.set(v) });
  }

  ngOnInit(): void {
    const salaryTypesObservable = this.salaryTypesService.get({});
    const sortingFunctionsObservable = this.sortingFunctionsService.get({});

    forkJoin([salaryTypesObservable, sortingFunctionsObservable]).subscribe({
      next: ([salaryTypesVal, sortingFunctionsVal]) => {

        this.salaryTypes.push(...salaryTypesVal);
        this.sortingFunctions.push(...sortingFunctionsVal);

        this.selectedSalaryType = this.salaryTypes[0];
        this.selectedSortingFunction = this.sortingFunctions[0];

        this.filterForm = this.formBuilder.group({
          salaryType: [this.selectedSalaryType],
          manualSalary: [50000],
          salaryMultiplicator: [1],
          sortingFunction: [this.selectedSortingFunction],
          sortAscending: [true]
        });

        this.filterForm.controls['salaryType']
        .valueChanges.subscribe(v => this.selectedSalaryType = v);

        this.filterForm.controls['sortingFunction']
        .valueChanges.subscribe(v => this.selectedSortingFunction = v);
      }
    });
  }

  getReport()
  {
    if (!this.filterForm)
      return;

    const currentSalaryType = this.selectedSalaryType;
    if (!this.filterForm.controls['salaryType'].valid || !currentSalaryType)
      return;

    const currentSortingFunction = this.selectedSortingFunction;
    if (!this.filterForm.controls['sortingFunction'].valid || !currentSortingFunction)
      return;

    var reportParams = {
      salaryTypeId: currentSalaryType.id,
      manualSalary: this.filterForm.controls['manualSalary'].value,
      salaryMultiplicator: this.filterForm.controls['salaryMultiplicator'].value,
      sortingFunctionId: currentSortingFunction.id,
      sortAscending: this.filterForm.controls['sortAscending'].value,
    };

    this.reportService.get(reportParams).subscribe({
      next: v => {
        this.reportSalaryType.set(currentSalaryType);
        this.reportSortingFunction.set(currentSortingFunction);
        this.reportResultItems.set(v.items);
      }
    });
  }

}
