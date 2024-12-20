import { Routes } from '@angular/router';
import { StartPageComponent } from './start-page/start-page.component';
import { AdminPageComponent } from './admin-page/admin-page.component';
import { LayoutComponent } from './layout/layout.component';
import { AdmCostOfLivingPageComponent } from './admin-page/adm-cost-of-living-page/adm-cost-of-living-page.component';
import { CostOfLivingPageComponent } from './data-analysis/cost-of-living-page/cost-of-living-page.component';
import { citiesResolver } from './services/cities.resolver';
import { cityDictionaryResolver } from './services/city-dictionary.resolver';
import { CitiesPlainPageComponent } from './data-analysis/cost-of-living-page/cities-plain-page/cities-plain-page.component';
import { citySalariesResolver } from './services/city-salaries.resolver';
import { regionTaxesResolver } from './services/region-taxes.resolver';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: '',
        component: StartPageComponent
      },
      {
        path: 'admin',
        children: [
          {
            path: '',
            component: AdminPageComponent
          },
          {
            path: 'cost-of-living',
            component: AdmCostOfLivingPageComponent
          }
        ]
      },
      {
        path: 'cost-of-living',
        component: CostOfLivingPageComponent,
        resolve: { cities: citiesResolver,
          dictionary: cityDictionaryResolver,
          salaries: citySalariesResolver,
          regionTaxes: regionTaxesResolver }
      },
      {
        path: 'city-costs-plain',
        component: CitiesPlainPageComponent,
        resolve: { cities: citiesResolver,
          dictionary: cityDictionaryResolver,
          salaries: citySalariesResolver,
          regionTaxes: regionTaxesResolver }
      },

    ]
  }
];
