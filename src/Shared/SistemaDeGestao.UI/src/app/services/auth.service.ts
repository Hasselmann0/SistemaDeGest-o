import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse, LoginUser } from '../models/auth.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private readonly apiUrl = 'https://localhost:7041/api/Auth';

  private readonly TOKEN_KEY = 'access_token';
  private readonly USER_KEY = 'current_user';
  private readonly REMEMBER_KEY = 'remember_me';

  readonly isLoggedIn = signal(this.hasToken());
  readonly currentUser = signal<LoginUser | null>(this.loadUser());

  private hasToken(): boolean {
    return !!localStorage.getItem(this.TOKEN_KEY) || !!sessionStorage.getItem(this.TOKEN_KEY);
  }

  private loadUser(): LoginUser | null {
    const data = localStorage.getItem(this.USER_KEY) || sessionStorage.getItem(this.USER_KEY);
    return data ? JSON.parse(data) : null;
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY) || sessionStorage.getItem(this.TOKEN_KEY);
  }

  login(credentials: LoginRequest, remember: boolean = false): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap((response) => {
        const storage = remember ? localStorage : sessionStorage;
        storage.setItem(this.TOKEN_KEY, response.token);
        storage.setItem(this.USER_KEY, JSON.stringify(response.user));

        if (remember) {
          localStorage.setItem(this.REMEMBER_KEY, 'true');
        } else {
          localStorage.removeItem(this.REMEMBER_KEY);
        }

        this.currentUser.set(response.user);
        this.isLoggedIn.set(true);
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    localStorage.removeItem(this.REMEMBER_KEY);
    sessionStorage.removeItem(this.TOKEN_KEY);
    sessionStorage.removeItem(this.USER_KEY);
    this.currentUser.set(null);
    this.isLoggedIn.set(false);
    this.router.navigate(['/login']);
  }

  shouldRemember(): boolean {
    return localStorage.getItem(this.REMEMBER_KEY) === 'true';
  }
}
