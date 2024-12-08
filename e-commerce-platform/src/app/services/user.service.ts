import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { User } from '../models/user.model';
import { AuthResponse } from '../models/auth-response.model';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = 'https://localhost:44376/api/v1/Login';
  private currentUserSubject: BehaviorSubject<User | null>;
  private tokenSubject: BehaviorSubject<string | null>;

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    if (isPlatformBrowser(this.platformId)) {
      this.currentUserSubject = new BehaviorSubject<User | null>(
        JSON.parse(localStorage.getItem('currentUser') || 'null')
      );
      this.tokenSubject = new BehaviorSubject<string | null>(
        localStorage.getItem('token')
      );
    } else {
      this.currentUserSubject = new BehaviorSubject<User | null>(null);
      this.tokenSubject = new BehaviorSubject<string | null>(null);
    }
  }

  public get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  public get token(): string | null {
    return this.tokenSubject.value;
  }

  public get isAuthenticated(): boolean {
    return !!this.currentUser && !!this.token;
  }

  // user.service.ts
  login(email: string, password: string): Observable<User> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, { email, password })
      .pipe(
        map((response) => {
          if (!response.isSuccess) {
            throw new Error(response.errorMessage || 'Login failed');
          }

          if (Array.isArray(response.data)) {
            const [token, user] = response.data;

            if (isPlatformBrowser(this.platformId)) {
              localStorage.setItem('currentUser', JSON.stringify(user));
              localStorage.setItem('token', token);
            }

            this.currentUserSubject.next(user);
            this.tokenSubject.next(token);
            return user;
          }

          throw new Error('Invalid response format');
        }),
        catchError((error) => {
          this.currentUserSubject.next(null);
          this.tokenSubject.next(null);
          return throwError(() => error);
        })
      );
  }

  register(user: Omit<User, 'userId'>): Observable<User> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, user).pipe(
      map((response) => {
        if (!response.isSuccess) {
          throw new Error(response.errorMessage || 'Registration failed');
        }

        if (isPlatformBrowser(this.platformId)) {
          localStorage.setItem(
            'currentUser',
            JSON.stringify(response.data.user)
          );
          localStorage.setItem('token', response.data.token);
        }

        this.currentUserSubject.next(response.data.user);
        this.tokenSubject.next(response.data.token);
        return response.data.user;
      }),
      catchError((error) => {
        console.error('Registration error:', error);
        this.currentUserSubject.next(null);
        this.tokenSubject.next(null);
        return throwError(() => error);
      })
    );
  }

  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('currentUser');
      localStorage.removeItem('token');
    }
    this.currentUserSubject.next(null);
    this.tokenSubject.next(null);
  }

  private handleError(error: any) {
    console.error('Auth error:', error);
    return throwError(
      () => new Error(error.error?.message || 'Authentication failed')
    );
  }
}
