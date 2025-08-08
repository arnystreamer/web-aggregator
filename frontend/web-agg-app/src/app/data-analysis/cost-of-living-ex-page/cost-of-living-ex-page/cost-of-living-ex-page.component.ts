import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ReportService } from '../../../services/report.service';
import { SalaryTypesService } from '../../../services/salary-types.service';
import { SortingFunctionsService } from '../../../services/sorting-functions.service';
import { SalaryTypeApi } from '../../../models/report/salary-type-api.model';
import { SortingFunctionApi } from '../../../models/report/sorting-function-api.model';
import { ReportRequestApi } from '../../../models/report/report-request-api.model';
import { forkJoin } from 'rxjs';
import { ReportCityExtendedApi } from '../../../models/report/report-city-extended-api.model';
import { CollectionApi } from '../../../models/collection-api.model';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { TermSpanComponent } from '../../../shared/term-span/term-span.component';
import { ProfitTaxableComponent } from '../../../shared/profit-taxable/profit-taxable.component';
import { CostFactComponent } from "../../../shared/cost-fact/cost-fact.component";
import { ProfitDesirableComponent } from "../../../shared/profit-desirable/profit-desirable.component";

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
    TermSpanComponent,
    ProfitTaxableComponent,
    CostFactComponent,
    ProfitDesirableComponent
],
  templateUrl: './cost-of-living-ex-page.component.html',
  styleUrl: './cost-of-living-ex-page.component.scss'
})
export class CostOfLivingExPageComponent implements OnInit {

  public filterForm?: FormGroup;

  public salaryTypes: SalaryTypeApi[] = [];
  public sortingFunctions: SortingFunctionApi[] = [];

  public reportResult?: CollectionApi<ReportCityExtendedApi>;
  public reportParams?: ReportRequestApi;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private reportService: ReportService,
    private salaryTypesService: SalaryTypesService, private sortingFunctionsService : SortingFunctionsService) {

  }

  ngOnInit(): void {
    const salaryTypesObservable = this.salaryTypesService.get({});
    const sortingFunctionsObservable = this.sortingFunctionsService.get({});

    forkJoin([salaryTypesObservable, sortingFunctionsObservable]).subscribe({
      next: ([salaryTypesVal, sortingFunctionsVal]) => {

        this.salaryTypes.push(...salaryTypesVal);
        this.sortingFunctions.push(...sortingFunctionsVal);

        this.filterForm = this.formBuilder.group({
          salaryTypeId: [this.salaryTypes[0].id],
          manualSalary: [50000],
          salaryMultiplicator: [1],
          sortingFunction: [this.sortingFunctions[0].id],
          sortAscending: [true]
        });
      }
    });
  }

  getReport()
  {
    if (!this.filterForm)
      return;

    if (!this.filterForm.controls['salaryTypeId'].valid)
      return;

    if (!this.filterForm.controls['sortingFunction'].valid)
      return;

    this.reportParams = {
      salaryTypeId: this.filterForm.controls['salaryTypeId'].value,
      manualSalary: this.filterForm.controls['manualSalary'].value,
      salaryMultiplicator: this.filterForm.controls['salaryMultiplicator'].value,
      sortingFunction: this.filterForm.controls['sortingFunction'].value,
      sortAscending: this.filterForm.controls['sortAscending'].value,
    };

    this.reportService.get(this.reportParams).subscribe({
      next: v => this.reportResult = v
    });
  }

}
