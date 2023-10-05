import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { LocalStorageService } from 'angular-web-storage';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private authService: AuthService
    , private router: Router, private localStorage: LocalStorageService) { }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
      if (!this.localStorage.get('Obj')) {
        this.router.navigate(['./login']);
        return false;
      }
      else if (route.data['hasRole'] &&
        route.data['hasRole'] !== this.localStorage.get('Obj').role) {
        this.router.navigate(['./accessdenied']);
        return false;
      }
      return true;
    }
}
