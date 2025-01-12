import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Product } from '../models/product.model';
import { StorageService } from './storage.service';

export interface PaginatedResponse {
  data: Product[];
  totalCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiURL = 'https://ecommerceproiect.site/api/v1/Products';

  constructor(private http: HttpClient, private storageService: StorageService) { }

  public getProducts(page: number = 1, pageSize: number = 9, category?: number, name?: string, stock?: number, price?: number): Observable<PaginatedResponse> {
    let params = new HttpParams()
      .set('Page', page.toString())
      .set('PageSize', pageSize.toString());

    if (category !== undefined && category !== null) {
      params = params.set('Category', category.toString());
    }
    if (name) {
      params = params.set('Name', name);
    }
    if (stock !== undefined && stock !== null) {
      params = params.set('Stock', stock.toString());
    }
    if (price !== undefined && price !== null) {
      params = params.set('Price', price.toString());
    }

    return this.http.get<PaginatedResponse>(`${this.apiURL}/paginated`, { params })
      .pipe(
        catchError(this.handleError)
      );
  }

  public createProduct(product: Product): Observable<any> {
    const token = this.storageService.getItem('token');
    const currentUser = this.storageService.getItem('currentUser'); // Get the current user from local storage

    if (!currentUser) {
      return throwError(() => new Error('User data not found in local storage'));
    }

    // Parse the currentUser object to get the userId
    let parsedUser: { userId: string } | null = null;
    try {
      parsedUser = JSON.parse(currentUser);
    } catch (e) {
      console.error('Error parsing user data:', e);
      return throwError(() => new Error('Invalid user data in local storage'));
    }

    if (!parsedUser || !parsedUser.userId) {
      return throwError(() => new Error('User ID not found in parsed user data'));
    }

    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();

    // Add the userId to the product object
    const productWithUserId = {
      ...product,
      userId: parsedUser.userId // Include the userId in the request payload
    };

    return this.http.post<any>(this.apiURL, productWithUserId, { headers })
      .pipe(
        catchError(this.handleError)
      );
  }

  public updateProduct(product: Product): Observable<any> {
    const token = this.storageService.getItem('token'); // Using StorageService
    console.log('Retrieved token:', token);
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.put<any>(this.apiURL, product, { headers })
      .pipe(
        catchError(this.handleError)
      );
  }

  public deleteProduct(id: string): Observable<any> {
    const token = this.storageService.getItem('token'); // Using StorageService
    console.log('Retrieved token:', token);
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.delete<any>(`${this.apiURL}/${id}`, { headers })
      .pipe(
        catchError(this.handleError)
      );
  }

  public getProductById(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiURL}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  public predictPrice(product: Product): Observable<any> {
    const url = 'https://ecommerceproiect.site/api/v1/ProductPricePrediction/predict';
    const requestBody = {
      name: product.name,
      description: product.description,
      price: 0,
      stock: product.stock,
      category: product.category
    };
    return this.http.post<any>(url, requestBody)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: HttpErrorResponse | Error) {
    let errorMessage = 'Unknown error!';

    if (error instanceof HttpErrorResponse) {
      // Server-side or network error
      errorMessage = error.error?.message || error.message;
    } else if (error instanceof Error) {
      // Client-side error
      errorMessage = error.message;
    }

    console.error('Error occurred:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
