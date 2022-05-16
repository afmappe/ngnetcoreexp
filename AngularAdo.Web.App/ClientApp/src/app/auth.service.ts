import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) {
  }

  login(email: string, password: string): Observable<boolean> {
    debugger
    var body = { userName: email, password:password };
    var subject = new Subject<boolean>();
    this.http.post('api/v1/Authentication/LoginUser', body, { responseType: 'text' })
      .subscribe((token: string) => {
        this.setSession(token);
        subject.next(true);
      });

    return subject.asObservable();
  }

  logout(): boolean {
    localStorage.removeItem("id_token");
    return true;
  }

  private setSession(token: string) {
    localStorage.setItem('id_token', token);
  }
}
