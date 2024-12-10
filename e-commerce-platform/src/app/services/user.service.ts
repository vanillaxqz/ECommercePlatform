import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { User } from '../models/user.model';
import { AuthResponse } from '../models/auth-response.model';
import { isPlatformBrowser } from '@angular/common';
import { StorageService } from './storage.service';

export interface UserRole {
  isGuest: boolean;
  isAuthenticated: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private _userRole: UserRole = {
    isGuest: false,
    isAuthenticated: false,
  };

  private apiUrl = 'https://ecommerceproiect.site/api/v1';
  private currentUserSubject: BehaviorSubject<User | null>;
  private tokenSubject: BehaviorSubject<string | null>;

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object,
    private storageService: StorageService
  ) {
    this.currentUserSubject = new BehaviorSubject<User | null>(
      JSON.parse(this.storageService.getItem('currentUser') || 'null')
    );
    this.tokenSubject = new BehaviorSubject<string | null>(
      this.storageService.getItem('token')
    );

    // Set initial authentication state
    this._userRole.isAuthenticated =
      !!this.currentUserSubject.value && !!this.tokenSubject.value;
  }

  get userRole(): UserRole {
    return this._userRole;
  }

  get isAuthenticated(): boolean {
    return this._userRole.isAuthenticated;
  }

  get isGuest(): boolean 
  {
    return this._userRole.isGuest;
  }

  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  get token(): string | null {
    return this.tokenSubject.value;
  }

  continueAsGuest(): void {
    this._userRole = { isGuest: true, isAuthenticated: false };
  }

  login(email: string, password: string): Observable<User> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/Auth/login`, { email, password })
      .pipe(
        map((response) => {
          if (!response.isSuccess) {
            throw new Error(response.errorMessage || 'Login failed');
          }

          if (Array.isArray(response.data)) {
            const [token, user] = response.data;

            this.storageService.setItem('currentUser', JSON.stringify(user));
            this.storageService.setItem('token', token);

            this.currentUserSubject.next(user);
            this.tokenSubject.next(token);
            this._userRole = { isGuest: false, isAuthenticated: true };
            return user;
          }

          throw new Error('Invalid response format');
        }),
        catchError((error) => {
          this.logout();
          return throwError(() => error);
        })
      );
  }

  register(user: Omit<User, 'userId'>): Observable<User> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Auth/register`, user).pipe(
      map((response) => {
        if (!response.isSuccess) {
          throw new Error(response.errorMessage || 'Registration failed');
        }

        this.storageService.setItem('currentUser', JSON.stringify(response.data.user));
        this.storageService.setItem('token', response.data.token);

        this.currentUserSubject.next(response.data.user);
        this.tokenSubject.next(response.data.token);
        this._userRole = { isGuest: false, isAuthenticated: true };
        return response.data.user;
      }),
      catchError((error) => {
        this.logout();
        return throwError(() => error);
      })
    );
  }

  logout(): void {
    this.storageService.removeItem('currentUser');
    this.storageService.removeItem('token');

    this.currentUserSubject.next(null);
    this.tokenSubject.next(null);
    this._userRole = { isGuest: false, isAuthenticated: false };
  }

  private handleError(error: any) {
    console.error('Auth error:', error);
    return throwError(() => new Error(error.error?.message || 'Authentication failed'));
  }
}
