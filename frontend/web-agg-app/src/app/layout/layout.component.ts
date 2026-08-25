import { Component, ChangeDetectionStrategy } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'wa-layout',
  imports: [
    RouterOutlet,
    MatToolbarModule
   ],
  templateUrl: './layout.component.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {

}
