import { Inject, Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpHeaders
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(@Inject('BASE_URL') private baseUrl: string,) { }

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const idToken = localStorage.getItem("id_token");

    var url = `${this.baseUrl}${req.url}`;

    if (idToken) {
      var cloned = req.clone({
        url: url,
        headers: req.headers
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${idToken}`)
      });

      return next.handle(cloned);
    }
    else {
      var cloned = req.clone({
        url: url,
        headers: req.headers
          .set('Content-Type', 'application/json')
      });
      return next.handle(cloned);
    }
  }
}
