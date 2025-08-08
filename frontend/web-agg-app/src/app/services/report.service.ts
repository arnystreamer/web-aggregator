import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReportRequestApi } from '../models/report/report-request-api.model';
import { ReportCityExtendedApi } from '../models/report/report-city-extended-api.model';
import { CollectionApi } from '../models/collection-api.model';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  private apiUrl = environment.apiUrl;
  private reportUrl = `${this.apiUrl}/api/report`;

  constructor(
    private httpClient: HttpClient) { }


  get(parameters: ReportRequestApi): Observable<CollectionApi<ReportCityExtendedApi>>
  {
    let params = new HttpParams();
    params = params.set("salaryTypeId", parameters.salaryTypeId);

    if (parameters.manualSalary)
      params = params.set("manualSalary", parameters.manualSalary);

    if (parameters.salaryMultiplicator)
      params = params.set("salaryMultiplicator", parameters.salaryMultiplicator);

    params = params.set("sortingFunction", parameters.sortingFunction);

    if (parameters.sortAscending !== undefined)
      params = params.set("sortAscending", parameters.sortAscending);

    return this.httpClient.get<CollectionApi<ReportCityExtendedApi>>(this.reportUrl, { params: params });
  }
}
