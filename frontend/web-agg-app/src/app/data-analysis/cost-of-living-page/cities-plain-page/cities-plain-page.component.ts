import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CityPlainPopulated } from '../../../models/city-plain-populated.model';
import { DictionaryDataItem } from '../../../models/dictionary-data-item.model';
import { City } from '../../../models/city.model';

@Component({
  selector: 'wa-cities-plain-page',
  imports: [
        MatButtonModule,
        MatIconModule,
        RouterModule
  ],
  templateUrl: './cities-plain-page.component.html',
  styleUrl: './cities-plain-page.component.scss'
})
export class CitiesPlainPageComponent  implements OnInit {
  public cities: CityPlainPopulated[] = [];
  public dictionaryItems: DictionaryDataItem[] = [];

  constructor(private route: ActivatedRoute)
  {

  }

  ngOnInit(): void {
    this.route.data.subscribe(({ cities, dictionary, salaries }) => {
      const rawCities: City[] = cities.items;
      this.dictionaryItems.push(...dictionary.items);

      for(let rawCity of rawCities)
      {
        this.cities.push(
        {
          id: rawCity.id,
          name: rawCity.name,
          region: rawCity.region,
          country: rawCity.country,
          dataItems: rawCity.dataItems.filter(di => di.value).map(di => {
            return { dictionaryItem: this.dictionaryItems.filter(v => v.id == di.dictionaryId)[0], ...di }
          })
        });
      }
    });
  }
}
