import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CheckoutService } from './checkout.service';
import { StorageService } from './storage.service';
import { HttpErrorResponse } from '@angular/common/http';
import { of, throwError } from 'rxjs';
import { User } from '../models/user.model';
import { PaymentRequest } from '../models/payment.model';

describe('CheckoutService', () => {
  let service: CheckoutService;
  let httpMock: HttpTestingController;
  let storageService: jasmine.SpyObj<StorageService>;

  beforeEach(() => {
    const storageServiceSpy = jasmine.createSpyObj('StorageService', ['getItem']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        CheckoutService,
        { provide: StorageService, useValue: storageServiceSpy }
      ]
    });

    service = TestBed.inject(CheckoutService);
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

    service.processCheckout().subscribe({
      next: () => fail('expected an error, not a success'),
      error: error => expect(error.message).toContain('User not authenticated')
    });

    expect(storageService.getItem).toHaveBeenCalledWith('currentUser');
    expect(storageService.getItem).toHaveBeenCalledWith('token');
  });

  it('should process checkout successfully', () => {
    const user: User = {
      userId: '1',
      name: 'John Doe',
      email: 'john.doe@example.com',
      phoneNumber: '1234567890',
      address: '123 Main St',
      password: 'password123'
    };
    const token = 'test-token';
    const paymentRequest: PaymentRequest = {
      paymentDate: new Date().toISOString(),
      userId: user.userId
    };

    storageService.getItem.and.callFake((key: string) => {
      if (key === 'currentUser') return JSON.stringify(user);
      if (key === 'token') return token;
      return null;
    });

    service.processCheckout().subscribe(response => {
      expect(response).toBeTruthy();
    });

    const paymentRequestCall = httpMock.expectOne(`${service['apiUrl']}/payments`);
    expect(paymentRequestCall.request.method).toBe('POST');
    expect(paymentRequestCall.request.headers.get('Authorization')).toBe(`Bearer ${token}`);
    paymentRequestCall.flush({ data: 'payment-id', isSuccess: true });

    const orderRequestCall = httpMock.expectOne(`${service['apiUrl']}/orders`);
    expect(orderRequestCall.request.method).toBe('POST');
    expect(orderRequestCall.request.headers.get('Authorization')).toBe(`Bearer ${token}`);
    orderRequestCall.flush({ data: 'order-id', isSuccess: true });
  });

  it('should handle error during payment creation', () => {
    const user: User = {
      userId: '1',
      name: 'John Doe',
      email: 'john.doe@example.com',
      phoneNumber: '1234567890',
      address: '123 Main St',
      password: 'password123'
    };
    const token = 'test-token';

    storageService.getItem.and.callFake((key: string) => {
      if (key === 'currentUser') return JSON.stringify(user);
      if (key === 'token') return token;
      return null;
    });

    service.processCheckout().subscribe({
      next: () => fail('expected an error, not a success'),
      error: error => expect(error.message).toContain('Payment creation failed')
    });

    const paymentRequestCall = httpMock.expectOne(`${service['apiUrl']}/payments`);
    expect(paymentRequestCall.request.method).toBe('POST');
    paymentRequestCall.flush({ message: 'Payment creation failed' }, { status: 500, statusText: 'Server Error' });
  });
});