// services/order.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { StorageService } from './storage.service';
import { ApiResponse } from '../models/api-response.model';
import { OrderDetails } from '../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = 'https://ecommerceproiect.site/api/v1';

  constructor(
    private http: HttpClient,
    private storageService: StorageService
  ) {}

  getOrderHistory(userId: string): Observable<ApiResponse<OrderDetails[]>> {
    const token = this.storageService.getItem('token');
    if (!token) {
      return throwError(() => new Error('User not authenticated'));
    }

    const headers = new HttpHeaders()
      .set('Authorization', `Bearer ${token}`)
      .set('Content-Type', 'application/json');

    return this.http.get<ApiResponse<OrderDetails[]>>(
      `${this.apiUrl}/Orders/User/${userId}`,
      { headers }
    ).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse | Error) {
    let errorMessage = 'Unknown error!';

    if (error instanceof HttpErrorResponse) {
      console.error('Full HTTP Error:', {
        status: error.status,
        statusText: error.statusText,
        error: error.error,
        message: error.message
      });
      errorMessage = error.error?.message || error.message;
    } else if (error instanceof Error) {
      console.error('Full Error:', error);
      errorMessage = error.message;
    }

    return throwError(() => new Error(errorMessage));
  }
}