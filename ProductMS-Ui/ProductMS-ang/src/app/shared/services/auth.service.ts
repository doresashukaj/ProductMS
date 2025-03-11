import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
baseURL ='https://localhost:7037/api/';
  constructor(private http: HttpClient) {}

  createUser(user: any): Observable<any> {
    return this.http.post(this.baseURL, user);
  }

  login(credentials: { email: string, password: string }): Observable<any> {
    return this.http.post(this.baseURL, credentials);
  }
}
