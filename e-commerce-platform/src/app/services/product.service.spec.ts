import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProductService, PaginatedResponse } from './product.service';
import { Product } from '../models/product.model';

describe('ProductService', () => {
  let service: ProductService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProductService]
    });
    service = TestBed.inject(ProductService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve products from the API via GET', () => {
    const dummyProducts: Product[] = [
      { productId: '1', name: 'Product 1', description: 'Description 1', price: 100, stock: 10, category: 1 },
      { productId: '2', name: 'Product 2', description: 'Description 2', price: 200, stock: 20, category: 2 }
    ];
    const paginatedResponse: PaginatedResponse = { data: dummyProducts, totalCount: 2 };

    service.getProducts().subscribe((products: PaginatedResponse) => {
      expect(products.data.length).toBe(2);
      expect(products.data).toEqual(dummyProducts);
    });

    const req = httpMock.expectOne(`${service['apiURL']}/paginated?Page=1&PageSize=10`);
    expect(req.request.method).toBe('GET');
    req.flush(paginatedResponse);
  });

  it('should create a new product via POST', () => {
    const newProduct: Product = { productId: '3', name: 'Product 3', description: 'Description 3', price: 300, stock: 30, category: 3 };

    service.createProduct(newProduct).subscribe((response: Product) => {
      expect(response).toEqual(newProduct);
    });

    const req = httpMock.expectOne(service['apiURL']);
    expect(req.request.method).toBe('POST');
    req.flush(newProduct);
  });

  it('should update an existing product via PUT', () => {
    const updatedProduct: Product = { productId: '1', name: 'Updated Product', description: 'Updated Description', price: 150, stock: 15, category: 1 };

    service.updateProduct(updatedProduct).subscribe((response: Product) => {
      expect(response).toEqual(updatedProduct);
    });

    const req = httpMock.expectOne(service['apiURL']);
    expect(req.request.method).toBe('PUT');
    req.flush(updatedProduct);
  });

  it('should delete a product via DELETE', () => {
    const productId = '1';

    service.deleteProduct(productId).subscribe((response: any) => {
      expect(response).toEqual({});
    });

    const req = httpMock.expectOne(`${service['apiURL']}/${productId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });

  it('should retrieve a product by id via GET', () => {
    const productId = '1';
    const product: Product = { productId: '1', name: 'Product 1', description: 'Description 1', price: 100, stock: 10, category: 1 };

    service.getProductById(productId).subscribe((response: Product) => {
      expect(response).toEqual(product);
    });

    const req = httpMock.expectOne(`${service['apiURL']}/${productId}`);
    expect(req.request.method).toBe('GET');
    req.flush(product);
  });

});