import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { DictionaryDataItem } from '../models/dictionary-data-item.model';
import { Observable } from 'rxjs';
import { CollectionApi } from '../models/collection-api.model';

@Injectable({
  providedIn: 'root'
})
export class CityDictionaryService {

  private apiUrl = environment.apiUrl;
  private cityDictionaryUrl = `${this.apiUrl}/api/city-dictionary`;

  constructor(
    private httpClient: HttpClient) { }

  getAll(skip?: number, take?: number): Observable<CollectionApi<DictionaryDataItem>>
  {
    let params = new HttpParams();
    if (skip)
      params = params.set("skip", skip);

    if (take)
      params = params.set("take", take);

    return this.httpClient.get<CollectionApi<DictionaryDataItem>>(this.cityDictionaryUrl, { params: params });
  }
}
