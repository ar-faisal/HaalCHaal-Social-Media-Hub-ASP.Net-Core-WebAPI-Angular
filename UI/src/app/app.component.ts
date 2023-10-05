import { Component } from '@angular/core';
import { LocalStorageService } from 'angular-web-storage';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  {
  title = 'HaalCHaal';

  constructor(
    public localStorage: LocalStorageService
  ) { }
}
