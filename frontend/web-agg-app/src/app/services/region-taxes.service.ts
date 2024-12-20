import { Injectable } from '@angular/core';
import { CollectionApi } from '../models/collection-api.model';
import { RegionTax } from '../models/region-tax.model';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RegionTaxesService {

  private apiUrl = environment.apiUrl;
  private cityDictionaryUrl = `${this.apiUrl}/api/region-taxes`;

  constructor(
    private httpClient: HttpClient) { }

  getAll(skip?: number, take?: number): Observable<CollectionApi<RegionTax>>
  {
    let params = new HttpParams();
    if (skip)
      params = params.set("skip", skip);

    if (take)
      params = params.set("take", take);

    return this.httpClient.get<CollectionApi<RegionTax>>(this.cityDictionaryUrl, { params: params });
  }
}
