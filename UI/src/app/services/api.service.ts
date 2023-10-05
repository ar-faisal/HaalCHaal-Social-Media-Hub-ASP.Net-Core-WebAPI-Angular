import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { StoryPost } from '../models/post';
import { LocalStorageService } from 'angular-web-storage';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  baseUrl = "https://localhost:5008/api/Posts";
  UrlFollowsg = "https://localhost:5008/api/User";

  constructor(private api: HttpClient, public localStorage: LocalStorageService) {
      
  }
  post(formData: FormData) {
    return this.api.post<StoryPost>(this.baseUrl + "/postss", formData);
  }

  getStoriesWithImages(): Observable<any[]> {
    
    return this.api.get<any>(this.baseUrl + "/stories");
  }

  getProfilePic()  {
    return this.api.get(this.baseUrl + "/getProfilePic/", { responseType: "blob" });
  }

  
  getProfileData(userId: string): Observable<any> {
    return this.api.get<any>(`${this.baseUrl}/profile/${userId}`);
  }

  





  isFollowing(userId: string) {
    return this.api.get<boolean>(`${this.UrlFollowsg}/isFollowing/${userId}`);

  }
  follow(userId: string) {
    return this.api.post<void>(`${this.UrlFollowsg}/follow/${userId}`, {});
  }
  unfollow(userId: string) {
    return this.api.post<void>(`${this.UrlFollowsg}/unfollow/${userId}`, {});
  }
  getPostsOfFollowedUsers(): Observable<any> {
    return this.api.get(`${this.UrlFollowsg}/GetPostsOfFollowedUsers`);
  }

}

