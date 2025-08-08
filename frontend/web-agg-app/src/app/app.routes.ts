import { Routes } from '@angular/router';
import { StartPageComponent } from './start-page/start-page.component';
import { AdminPageComponent } from './admin-page/admin-page.component';
import { LayoutComponent } from './layout/layout.component';
import { AdmCostOfLivingPageComponent } from './admin-page/adm-cost-of-living-page/adm-cost-of-living-page.component';
import { CostOfLivingExPageComponent } from './data-analysis/cost-of-living-ex-page/cost-of-living-ex-page.component';

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
        path: 'cost-of-living-extended',
        component: CostOfLivingExPageComponent,
      }
    ]
  }
];
