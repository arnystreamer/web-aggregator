import { Injectable } from '@angular/core';
import { CollectionApi } from '../models/collection-api.model';
import { CitySalary } from '../models/city-salary.model';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CitySalariesService {

  private apiUrl = environment.apiUrl;
  private cityDictionaryUrl = `${this.apiUrl}/api/city-salaries`;

  constructor(
    private httpClient: HttpClient) { }

  getAll(skip?: number, take?: number): Observable<CollectionApi<CitySalary>>
  {
    let params = new HttpParams();
    if (skip)
      params = params.set("skip", skip);

    if (take)
      params = params.set("take", take);

    return this.httpClient.get<CollectionApi<CitySalary>>(this.cityDictionaryUrl, { params: params });
  }
}
