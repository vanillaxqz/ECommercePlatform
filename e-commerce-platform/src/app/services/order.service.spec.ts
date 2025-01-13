import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { OrderService } from './order.service';
import { StorageService } from './storage.service';
import { HttpErrorResponse } from '@angular/common/http';
import { of, throwError } from 'rxjs';
import { ApiResponse } from '../models/api-response.model';
import { OrderDetails } from '../models/order.model';

describe('OrderService', () => {
  let service: OrderService;
  let httpMock: HttpTestingController;
  let storageService: jasmine.SpyObj<StorageService>;

  beforeEach(() => {
    const storageServiceSpy = jasmine.createSpyObj('StorageService', ['getItem']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        OrderService,
        { provide: StorageService, useValue: storageServiceSpy }
      ]
    });

    service = TestBed.inject(OrderService);
    httpMock = TestBed.inject(HttpTestingController);
    storageService = TestBed.inject(StorageService) as jasmine.SpyObj<StorageService>;
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should throw error if user is not authenticated', () => {
    storageService.getItem.and.returnValue(null);

    service.getOrderHistory('1').subscribe({
      next: () => fail('expected an error, not a success'),
      error: error => expect(error.message).toContain('User not authenticated')
    });

    expect(storageService.getItem).toHaveBeenCalledWith('token');
  });

  it('should handle error during get order history', () => {
    const token = 'test-token';

    storageService.getItem.and.returnValue(token);

    service.getOrderHistory('1').subscribe({
      next: () => fail('expected an error, not a success'),
      error: error => expect(error.message).toContain('Failed to load order history')
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/Orders/User/1`);
    expect(req.request.method).toBe('GET');
    req.flush({ message: 'Failed to load order history' }, { status: 500, statusText: 'Server Error' });
  });
});