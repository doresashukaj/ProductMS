import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7037/api/accounts/';
  private tokenKey = 'auth_token';  // Ensure token key is consistent across methods

  constructor(private http: HttpClient) { }

  // Method to register a new user
  createUser(userData: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, userData);
  }

  // Login method that returns the response
  login(credentials: { email: string, password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}login`, credentials);
  }

  // Method to check if the user is authenticated
  isAuthenticated(): boolean {
    return localStorage.getItem(this.tokenKey) !== null; 
  }

  // Retrieve the stored token
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // Save token to localStorage
  setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  // Method to get headers with Authorization token
  getAuthHeaders() {
    const token = this.getToken();
    if (token) {
      return new HttpHeaders().set('Authorization', `Bearer ${token}`);
    }
    return null;
  }

  // Check if the user is logged in
  isLoggedIn(): boolean {
    return localStorage.getItem(this.tokenKey) !== null;
  }

  // Delete the token from localStorage
  deleteToken(): void {
    localStorage.removeItem(this.tokenKey);
  }
}
