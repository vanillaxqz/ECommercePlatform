import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UserService } from './user.service';
import { StorageService } from './storage.service';
import { User } from '../models/user.model';
import { AuthResponse, UserResponse } from '../models/auth-response.model';
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject } from 'rxjs';

describe('UserService', () => {
  let service: UserService;
  let httpMock: HttpTestingController;
  let storageService: jasmine.SpyObj<StorageService>;
  let platformId: Object;

  beforeEach(() => {
    platformId = 'browser'; // Mock platform ID as 'browser'
    const storageServiceSpy = jasmine.createSpyObj('StorageService', ['getItem', 'setItem', 'removeItem']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        UserService,
        { provide: PLATFORM_ID, useValue: platformId },
        { provide: StorageService, useValue: storageServiceSpy }
      ]
    });

    service = TestBed.inject(UserService);
    httpMock = TestBed.inject(HttpTestingController);
    storageService = TestBed.inject(StorageService) as jasmine.SpyObj<StorageService>;
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize user from stored user data', () => {
    const storedUser = JSON.stringify({ userId: '1', name: 'John Doe', email: 'john.doe@example.com', phoneNumber: '1234567890', address: '123 Main St', password: 'password123' });
    storageService.getItem.and.returnValue(storedUser);

    service = TestBed.inject(UserService);
    expect(service.currentUser).toEqual(JSON.parse(storedUser));
  });

  it('should initialize user from stored user ID', () => {
    const storedUserId = '1';
    storageService.getItem.and.returnValue(storedUserId);

    service = TestBed.inject(UserService);
    expect(service.currentUser).toBeNull();

    const req = httpMock.expectOne(`${service['apiUrl']}/Users/${storedUserId}`);
    expect(req.request.method).toBe('GET');
    req.flush({ userId: '1', name: 'John Doe', email: 'john.doe@example.com', phoneNumber: '1234567890', address: '123 Main St', password: 'password123' });

    expect(service.currentUser).toEqual({ userId: '1', name: 'John Doe', email: 'john.doe@example.com', phoneNumber: '1234567890', address: '123 Main St', password: 'password123' });
  });

  it('should handle error when initializing user from stored user ID', () => {
    const storedUserId = '1';
    storageService.getItem.and.returnValue(storedUserId);

    service = TestBed.inject(UserService);
    expect(service.currentUser).toBeNull();

    const req = httpMock.expectOne(`${service['apiUrl']}/Users/${storedUserId}`);
    expect(req.request.method).toBe('GET');
    req.flush({ message: 'User not found' }, { status: 404, statusText: 'Not Found' });

    expect(service.currentUser).toBeNull();
  });

  it('should handle error during authentication', () => {
    service.login('john.doe@example.com', 'wrongpassword').subscribe({
      next: () => fail('expected an error, not a success'),
      error: error => expect(error.message).toContain('Authentication failed')
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/auth/login`);
    expect(req.request.method).toBe('POST');
    req.flush({ message: 'Authentication failed' }, { status: 401, statusText: 'Unauthorized' });

    expect(service.currentUser).toBeNull();
    expect(service.token).toBeNull();
  });

  it('should logout user and clear storage', () => {
    service.logout();

    expect(service.currentUser).toBeNull();
    expect(service.token).toBeNull();
    expect(storageService.removeItem).toHaveBeenCalledWith('currentUser');
    expect(storageService.removeItem).toHaveBeenCalledWith('token');
  });
});