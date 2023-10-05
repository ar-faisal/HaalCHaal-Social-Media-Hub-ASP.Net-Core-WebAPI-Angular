import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { LocalStorageService } from 'angular-web-storage';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  

  imageToShow: any;
  
  constructor(private authService: AuthService, private apiService: ApiService,
    private router: Router,
    public localStorage: LocalStorageService
  ) {}

  ngOnInit(): void {

   
      if (this.localStorage.get('Obj')) { this.loadProfilePic(); }
   
      

      


  }
  profileId() {

    var Id = this.localStorage.get('Obj');
    Id = Id.id;
   

    this.router.navigate(['./profile', Id]);
  }

  loadProfilePic() {
   
    this.apiService.getProfilePic().subscribe(res => {
      let reader = new FileReader();
      reader.addEventListener("load", x => { this.imageToShow = reader.result; });
      reader.readAsDataURL(res);
      
    });

    }

  logout() {
    this.authService.logout().subscribe(
      res => {
      },
      err => { },
      () => {
        this.localStorage.remove('Obj');
        this.imageToShow = {};
        this.router.navigate(['./login']);
      }
    );
  }
}
