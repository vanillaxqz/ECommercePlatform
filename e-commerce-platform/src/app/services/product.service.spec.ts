import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProductService, PaginatedResponse } from './product.service';
import { StorageService } from './storage.service';
import { Product } from '../models/product.model';

describe('ProductService', () => {
  let service: ProductService;
  let httpMock: HttpTestingController;
  let storageService: jasmine.SpyObj<StorageService>;

  beforeEach(() => {
    const storageServiceSpy = jasmine.createSpyObj('StorageService', ['getItem']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        ProductService,
        { provide: StorageService, useValue: storageServiceSpy }
      ]
    });

    service = TestBed.inject(ProductService);
    httpMock = TestBed.inject(HttpTestingController);
    storageService = TestBed.inject(StorageService) as jasmine.SpyObj<StorageService>;
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get products with default parameters', () => {
    const paginatedResponse: PaginatedResponse = {
      data: [{ productId: '1', name: 'Product 1', price: 100, stock: 10, category: 1, description: 'Description 1', userId: '1' }],
      totalCount: 1
    };

    service.getProducts().subscribe(response => {
      expect(response).toEqual(paginatedResponse);
    });

    const req = httpMock.expectOne(`${service['apiURL']}/paginated?Page=1&PageSize=9`);
    expect(req.request.method).toBe('GET');
    req.flush(paginatedResponse);
  });

  it('should get products with specified parameters', () => {
    const paginatedResponse: PaginatedResponse = {
      data: [{ productId: '1', name: 'Product 1', price: 100, stock: 10, category: 1, description: 'Description 1', userId: '1' }],
      totalCount: 1
    };

    service.getProducts(2, 5, 1, 'Product 1', 10, 100).subscribe(response => {
      expect(response).toEqual(paginatedResponse);
    });

    const req = httpMock.expectOne(`${service['apiURL']}/paginated?Page=2&PageSize=5&Category=1&Name=Product%201&Stock=10&Price=100`);
    expect(req.request.method).toBe('GET');
    req.flush(paginatedResponse);
  });

  it('should handle error when getting products', () => {
    service.getProducts().subscribe({
      next: () => fail('expected an error, not a success'),
      error: error => expect(error.message).toContain('Failed to load products')
    });

    const req = httpMock.expectOne(`${service['apiURL']}/paginated?Page=1&PageSize=9`);
    expect(req.request.method).toBe('GET');
    req.flush({ message: 'Failed to load products' }, { status: 500, statusText: 'Server Error' });
  });

  it('should create a product successfully', () => {
    const product: Product = { productId: '1', name: 'Product 1', price: 100, stock: 10, category: 1, description: 'Description 1', userId: '1' };
    const token = 'test-token';
    const currentUser = JSON.stringify({ userId: '1', name: 'John Doe' });

    storageService.getItem.and.callFake((key: string) => {
      if (key === 'token') return token;
      if (key === 'currentUser') return currentUser;
      return null;
    });

    service.createProduct(product).subscribe(response => {
      expect(response).toEqual({ success: true });
    });

    const req = httpMock.expectOne(`${service['apiURL']}`);
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe(`Bearer ${token}`);
    req.flush({ success: true });
  });

  it('should handle error when creating a product', () => {
    const product: Product = { productId: '1', name: 'Product 1', price: 100, stock: 10, category: 1, description: 'Description 1', userId: '1' };
    const token = 'test-token';
    const currentUser = JSON.stringify({ userId: '1', name: 'John Doe' });

    storageService.getItem.and.callFake((key: string) => {
      if (key === 'token') return token;
      if (key === 'currentUser') return currentUser;
      return null;
    });

    service.createProduct(product).subscribe({
      next: () => fail('expected an error, not a success'),
      error: error => expect(error.message).toContain('Failed to create product')
    });

    const req = httpMock.expectOne(`${service['apiURL']}`);
    expect(req.request.method).toBe('POST');
    req.flush({ message: 'Failed to create product' }, { status: 500, statusText: 'Server Error' });
  });
});