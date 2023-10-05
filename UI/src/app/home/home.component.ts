import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageService } from 'angular-web-storage';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  posts: any[] = [];

  constructor(private router: Router,
    public localStorage: LocalStorageService,private api: ApiService
  ) { }


  ngOnInit(): void {
        
    if (!this.localStorage.get('Obj'))
    { this.router.navigate(['./login']); }

    
    this.api.getPostsOfFollowedUsers().subscribe((data) => {
      this.posts = data;
    });
  
    
  }
  profileId(userId: string) {
    console.log(userId)
     this.router.navigate(['./profile', userId]);
  }
  

}
