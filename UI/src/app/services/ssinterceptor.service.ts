import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { LocalStorageService } from "angular-web-storage";
import { Observable } from "rxjs";


@Injectable({
  providedIn: 'root'
})
export class SsinterceptorService implements HttpInterceptor {

  constructor(
    private localStorage: LocalStorageService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    
    if (this.localStorage.get("Obj")) {
      const AuthRequest = req.clone({ headers: new HttpHeaders().set("Authorization", "Bearer " + this.localStorage.get("Obj").tokenJwt) });
      return next.handle(AuthRequest)
    }
    else {
      return next.handle(req);
    }

  }
}
