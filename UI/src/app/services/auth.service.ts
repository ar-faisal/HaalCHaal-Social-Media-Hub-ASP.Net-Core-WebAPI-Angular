import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Hcuser } from '../models/hcuser';
import { Loginvm } from '../models/loginvm';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = "https://localhost:5008/api/Accounts";
  constructor(private api: HttpClient) { }

  register(formData: FormData) {
    return this.api.post<Hcuser>(this.baseUrl + "/register", formData);
  }
  login(loginUser: Loginvm) {
    return this.api.post<Loginvm>(this.baseUrl + "/login", loginUser, { withCredentials: true }); 
  }

  logout() {
    return this.api.post<any>(this.baseUrl + "/logout", null, { withCredentials: true });
  }
}
