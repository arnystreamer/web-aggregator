import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { environment } from '../../environments/environment';


@Component({
  selector: 'wa-start-page',
  imports: [
    RouterModule,
    MatCardModule,
    MatChipsModule
  ],
  templateUrl: './start-page.component.html',
  styleUrl: './start-page.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StartPageComponent {
  public version = environment.version;
  public prodFlag = environment.production ? 'P' : '';
}
