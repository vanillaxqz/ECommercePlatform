import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http'
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';
import { PaginatedResult } from '../models/paginated-result.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiURL = 'http://localhost:5270/api/Products'

  constructor(private http: HttpClient) {
  }

  //GET: /api/Products/{id}
  //GET: /api/Products/paginated
  //DELETE: /api/Products/{id}
  //PUT: /api/Products
  //POST: /api/Products

  public getProducts(pageNumber: number = 1, pageSize: number = 10): Observable<PaginatedResult<Product>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<PaginatedResult<Product>>(`${this.apiURL}/paginated`, { params });
  }

  public createProduct(product: Product): Observable<any> {
    return this.http.post<Product>(this.apiURL, product);
  }

  public updateProduct(product: Product): Observable<any> {
    return this.http.put<Product>(this.apiURL, product);
  }

  public deleteProduct(id: number): Observable<any> {
    return this.http.delete(`${this.apiURL}/${id}`);
  }

  public getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiURL}/${id}`);
  }

}
