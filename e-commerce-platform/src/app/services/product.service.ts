import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';

export interface PaginatedResponse {
  data: Product[];
  totalCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiURL = 'https://ecommerceproiect.site/api/Products';

  constructor(private http: HttpClient) { }

  public getProducts(page: number = 1, pageSize: number = 10, category?: number): Observable<PaginatedResponse> {
    let params = new HttpParams()
      .set('Page', page.toString())
      .set('PageSize', pageSize.toString());
    
    if (category !== undefined && category !== null) {
      params = params.set('Category', category.toString());
    }
      
    return this.http.get<PaginatedResponse>(`${this.apiURL}/paginated`, { params });
  }

  public createProduct(product: Product): Observable<any> {
    return this.http.post<any>(this.apiURL, product);
  }

  public updateProduct(product: Product): Observable<any> {
    return this.http.put<any>(this.apiURL, product);
  }

  public deleteProduct(id: string): Observable<any> {
    return this.http.delete<any>(`${this.apiURL}/${id}`);
  }

  public getProductById(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiURL}/${id}`);
  }
}