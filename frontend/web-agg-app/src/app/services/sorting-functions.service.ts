import { Injectable } from '@angular/core';
import { CollectionRequestApi } from '../models/collection-request-api.model';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { SortingFunctionApi } from '../models/report/sorting-function-api.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SortingFunctionsService {

  private apiUrl = environment.apiUrl;
  private sortingFunctionsUrl = `${this.apiUrl}/api/sorting-functions`;

  constructor(
    private httpClient: HttpClient) { }

  get(parameters: CollectionRequestApi) : Observable<SortingFunctionApi[]>
  {
    let params = new HttpParams();
    if (parameters.skip)
      params = params.set("skip", parameters.skip);

    if (parameters.take)
      params = params.set("take", parameters.take);

    return this.httpClient.get<SortingFunctionApi[]>(this.sortingFunctionsUrl, { params: params });
  }
}
