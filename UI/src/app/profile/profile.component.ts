import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit  {
  userId: string = "";
  profileData: any;
  isFollowing: any;

  constructor(private route: ActivatedRoute, private apiService: ApiService) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {      
      this.userId = params['id'];

      this.apiService.isFollowing(this.userId).subscribe((data) => {
        // Parse the JSON response and set the isFollowing variable
        this.isFollowing = data;
        console.log(this.isFollowing);
      });


      this.apiService.getProfileData(this.userId).subscribe(
        data => { this.profileData = data; },
        error => { console.error('Error:', error);
        });

    });
  }
  follow() {
    
    this.apiService.follow(this.userId).subscribe(
      
      error => {
        console.error('Error:', error)
      },
      () => { this.isFollowing.isFollowing = true; }
    );

  }
  unfollow() {
    this.apiService.unfollow(this.userId).subscribe(

      
      () => {
        
        this.isFollowing.isFollowing = false;
        console.log("srsr");      }
    );
  } 
}
