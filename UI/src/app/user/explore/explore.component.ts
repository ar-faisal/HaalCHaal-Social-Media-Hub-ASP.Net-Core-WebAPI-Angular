import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-explore',
  templateUrl: './explore.component.html',
  styleUrls: ['./explore.component.css']
})
export class ExploreComponent implements OnInit {

    storiesWithImages: any[] = []; 

  constructor(private api: ApiService, private router: Router) { }

    ngOnInit(): void {
            this.api.getStoriesWithImages().subscribe((data) => {
            this.storiesWithImages = data.reverse();
              
      });
    }

  profileId(userId: string) {

   


    this.router.navigate(['./profile', userId]);
  }
}
