import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = `http://localhost:5000/auth`;
  constructor(private http: HttpClient) { }
  public login(model: any) {
    return this.http.post(`${this.baseUrl}/login`, model).pipe(
      map((response: any) => {
        const user = response;
        localStorage.setItem('token', user.token);
      })
    );
  }
  public register(model: any) {
    return this.http.post(`${this.baseUrl}/register`, model);
  }
}
