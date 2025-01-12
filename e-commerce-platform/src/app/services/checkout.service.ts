import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators';
import { StorageService } from './storage.service';
import { ApiResponse } from '../models/api-response.model';
import { OrderDetails, OrderRequest } from '../models/order.model';
import { PaymentRequest } from '../models/payment.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  private apiUrl = 'https://ecommerceproiect.site/api/v1';

  constructor(
    private http: HttpClient,
    private storageService: StorageService
  ) {}

  processCheckout(): Observable<string> {
    const currentUser = JSON.parse(this.storageService.getItem('currentUser') || '{}') as User;
    const token = this.storageService.getItem('token');
    
    console.log('Current User:', currentUser);
    console.log('Token present:', !!token);
    
    if (!currentUser.userId || !token) {
      return throwError(() => new Error('User not authenticated'));
    }

    const headers = new HttpHeaders()
      .set('Authorization', `Bearer ${token}`)
      .set('Content-Type', 'application/json');

    const paymentData: PaymentRequest = {
      paymentDate: new Date().toISOString(),
      userId: currentUser.userId
    };

    console.log('Payment Request Data:', paymentData);

    return this.createPayment(paymentData, headers).pipe(
      tap(paymentId => console.log('Payment created with ID:', paymentId)),
      switchMap(paymentId => {
        const orderData: OrderRequest = {
          userId: currentUser.userId,
          orderDate: new Date().toISOString(),
          status: 1,
          paymentId: paymentId
        };
        console.log('Creating order with data:', orderData);
        return this.createOrder(orderData, headers);
      }),
      tap(orderId => console.log('Order created with ID:', orderId)),
      catchError(error => {
        console.error('Process checkout error:', error);
        return this.handleError(error);
      })
    );
  }

  getOrderDetails(orderId: string): Observable<ApiResponse<OrderDetails>> {
    const token = this.storageService.getItem('token');
    if (!token) {
      return throwError(() => new Error('User not authenticated'));
    }

    const headers = new HttpHeaders()
      .set('Authorization', `Bearer ${token}`)
      .set('Content-Type', 'application/json');
    
    return this.http.get<ApiResponse<OrderDetails>>(
      `${this.apiUrl}/Orders/${orderId}`,
      { headers }
    ).pipe(
      catchError(this.handleError)
    );
  }

  private createPayment(paymentData: PaymentRequest, headers: HttpHeaders): Observable<string> {
    return this.http.post<string>(
      `${this.apiUrl}/Payments`, 
      paymentData,
      { headers }
    ).pipe(
      catchError(error => {
        console.error('Create payment error:', error);
        return throwError(() => error);
      })
    );
  }

  private createOrder(orderData: OrderRequest, headers: HttpHeaders): Observable<string> {
    return this.http.post<string>(
      `${this.apiUrl}/Orders`,
      orderData,
      { headers }
    ).pipe(
      catchError(error => {
        console.error('Create order error:', error);
        return throwError(() => error);
      })
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