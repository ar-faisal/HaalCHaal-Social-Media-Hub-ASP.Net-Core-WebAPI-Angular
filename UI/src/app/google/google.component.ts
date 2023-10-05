import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LocalStorageService } from 'angular-web-storage';

@Component({
  selector: 'app-google',
  templateUrl: './google.component.html',
  styleUrls: ['./google.component.css']
})
export class GoogleComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute,
    private localStorage: LocalStorageService,
    private router: Router
  ) { }

    ngOnInit(): void {
      this.activatedRoute.queryParams.subscribe(params => {
        if (params['tokenJwt']) {
          let obj = {
            "tokenJwt": params['tokenJwt'],
            "id": params['id'],
            "userName": params['userName'],
            "role": params['role']
          };
          this.localStorage.set("Obj", obj);
          this.router.navigate(['home']);
        }
      });
    }

}
