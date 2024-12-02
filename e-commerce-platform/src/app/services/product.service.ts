import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';
import { PaginatedResult } from '../models/paginated-result.model';

export interface ApiResponse<T> {
  isSuccess: boolean;
  data: T;
  errorMessage?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiURL = 'http://localhost:5270/api/Products';

  constructor(private http: HttpClient) { }

  public getProducts(pageNumber: number = 1, pageSize: number = 10): Observable<ApiResponse<Product[]>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
      
    return this.http.get<ApiResponse<Product[]>>(this.apiURL, { params });
  }

  public createProduct(product: Product): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiURL, product);
  }

  public updateProduct(product: Product): Observable<ApiResponse<void>> {
    return this.http.put<ApiResponse<void>>(this.apiURL, product);
  }

  public deleteProduct(id: string): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiURL}/${id}`);
  }

  public getProductById(id: string): Observable<ApiResponse<Product>> {
    return this.http.get<ApiResponse<Product>>(`${this.apiURL}/${id}`);
  }
}