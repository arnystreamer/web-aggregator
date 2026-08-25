import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'wa-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'web-agg-app';
}
