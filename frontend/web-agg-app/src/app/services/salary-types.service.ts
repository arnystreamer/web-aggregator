import { Injectable } from '@angular/core';
import { CollectionRequestApi } from '../models/collection-request-api.model';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { SalaryTypeApi } from '../models/report/salary-type-api.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SalaryTypesService {

  private apiUrl = environment.apiUrl;
  private salaryTypesUrl = `${this.apiUrl}/api/salary-types`;

  constructor(
    private httpClient: HttpClient) { }

  get(parameters: CollectionRequestApi) : Observable<SalaryTypeApi[]>
  {
    let params = new HttpParams();
    if (parameters.skip)
      params = params.set("skip", parameters.skip);

    if (parameters.take)
      params = params.set("take", parameters.take);

    return this.httpClient.get<SalaryTypeApi[]>(this.salaryTypesUrl, { params: params });
  }
}
