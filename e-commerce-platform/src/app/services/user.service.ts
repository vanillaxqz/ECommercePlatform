import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { User } from '../models/user.model';
import { AuthResponse, UserResponse } from '../models/auth-response.model';
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
    isGuest: true,
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
    this.tokenSubject = new BehaviorSubject<string | null>(
      this.storageService.getItem('token')
    );

    const storedUser = this.storageService.getItem('currentUser');
    this.currentUserSubject = new BehaviorSubject<User | null>(null);

    if (storedUser) {
      try {
        const parsedUser = JSON.parse(storedUser);
        if (typeof parsedUser === 'string') {
          // If it's just an ID, fetch the full user data
          this.initializeUserFromId(parsedUser);
        } else if (parsedUser && parsedUser.userId) {
          // If it's a complete user object
          this.currentUserSubject.next(parsedUser);
          // Optionally refresh user data
          this.refreshUserData(parsedUser.userId);
        }
      } catch (e) {
        // If parsing fails, try to use the raw value as userId
        if (storedUser) {
          this.initializeUserFromId(storedUser);
        }
      }
    }

    // Set initial authentication state
    this._userRole.isAuthenticated =
      !!this.currentUserSubject.value && !!this.tokenSubject.value;
  }

  private initializeUserFromId(userId: string): void {
    // Create a temporary partial user object
    const partialUser: User = {
      userId: userId,
      name: '',
      email: '',
      password: '',
      address: '',
      phoneNumber: ''
    };
    
    // Set the partial user immediately
    this.currentUserSubject.next(partialUser);
    
    // Fetch complete user data
    this.getUserById(userId).subscribe({
      next: (fullUser) => {
        this.storageService.setItem('currentUser', JSON.stringify(fullUser));
        this.currentUserSubject.next(fullUser);
        this._userRole = { isGuest: false, isAuthenticated: true };
      },
      error: (error) => {
        console.error('Error fetching user data:', error);
        // Don't logout here, keep the partial user data
      }
    });
  }

  private refreshUserData(userId: string): void {
    this.getUserById(userId).subscribe({
      next: (user) => {
        this.storageService.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        this._userRole = { isGuest: false, isAuthenticated: true };
      },
      error: (error) => {
        console.error('Error refreshing user data:', error);
      }
    });
  }

  get userRole(): UserRole {
    return this._userRole;
  }

  get isAuthenticated(): boolean {
    return this._userRole.isAuthenticated;
  }

  get isGuest(): boolean {
    return this._userRole.isGuest;
  }

  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  get token(): string | null {
    return this.tokenSubject.value;
  }

  get currentUserObservable(): Observable<User | null> {
    return this.currentUserSubject.asObservable();
  }

  continueAsGuest(): void {
    this._userRole = { isGuest: true, isAuthenticated: false };
  }

  requestPasswordReset(email: string): Observable<any> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Auth/initiate-password-reset`, { email })
      .pipe(
        map(response => {
          if (!response.isSuccess) {
            throw new Error(response.errorMessage || 'Failed to send reset email');
          }
          return response.data;
        }),
        catchError(this.handleError)
      );
  }

  resetPassword(resetToken: string, newPassword: string): Observable<any> {
    console.log('Reset token:', resetToken);
    const headers = new HttpHeaders().set('Authorization', `Bearer ${resetToken}`);

    return this.http.post<AuthResponse>(
      `${this.apiUrl}/Auth/reset-password`, 
      { newPassword },
      { headers }
    ).pipe(
      map(response => {
        if (!response.isSuccess) {
          throw new Error(response.errorMessage || 'Password reset failed');
        }
        return response.data;
      }),
      catchError(this.handleError)
    );
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

  getUserById(userId: string): Observable<User> {
    return this.http.get<UserResponse>(`${this.apiUrl}/Users/${userId}`).pipe(
      map(response => {
        if (!response.isSuccess) {
          throw new Error(response.errorMessage || 'Failed to fetch user data');
        }
        return response.data;
      }),
      tap(user => {
        // Update stored user data when we get fresh data
        if (user && user.userId) {
          this.storageService.setItem('currentUser', JSON.stringify(user));
        }
      }),
      catchError(this.handleError)
    );
  }

  editUser(userData: Omit<User, 'userId'>): Observable<User> {
    if (!this.currentUser?.userId) {
      return throwError(() => new Error('No user ID available'));
    }

    return this.http.post<UserResponse>(`${this.apiUrl}/Users`, {
      ...userData,
      userId: this.currentUser.userId
    }).pipe(
      map(response => {
        if (!response.isSuccess) {
          throw new Error(response.errorMessage || 'Failed to update user data');
        }
        const updatedUser = response.data;
        this.storageService.setItem('currentUser', JSON.stringify(updatedUser));
        this.currentUserSubject.next(updatedUser);
        return updatedUser;
      }),
      catchError(this.handleError)
    );
  }

  logout(): void {
    this.storageService.removeItem('currentUser');
    this.storageService.removeItem('token');
    this.currentUserSubject.next(null);
    this.tokenSubject.next(null);
    this._userRole = { isGuest: true, isAuthenticated: false };
  }

  private handleError(error: any) {
    console.error('Auth error:', error);
    return throwError(() => new Error(error.error?.message || 'Authentication failed'));
  }
}