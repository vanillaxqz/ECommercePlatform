import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserProfileComponent } from './user-profile.component';
import { UserService } from '../../services/user.service';
import { OrderService } from '../../services/order.service';
import { RouterTestingModule } from '@angular/router/testing';
import { of, throwError, Subject } from 'rxjs';
import { User } from '../../models/user.model';
import { OrderDetails } from '../../models/order.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

describe('UserProfileComponent', () => {
  let component: UserProfileComponent;
  let fixture: ComponentFixture<UserProfileComponent>;
  let userService: jasmine.SpyObj<UserService>;
  let orderService: jasmine.SpyObj<OrderService>;
  let currentUserSubject: Subject<string>;

  beforeEach(async () => {
    currentUserSubject = new Subject<string>();
    const userServiceSpy = jasmine.createSpyObj('UserService', ['getUserById'], {
      currentUserObservable: currentUserSubject.asObservable()
    });
    const orderServiceSpy = jasmine.createSpyObj('OrderService', ['getOrderHistory']);

    await TestBed.configureTestingModule({
      declarations: [UserProfileComponent],
      imports: [CommonModule, RouterTestingModule, FormsModule],
      providers: [
        { provide: UserService, useValue: userServiceSpy },
        { provide: OrderService, useValue: orderServiceSpy }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserProfileComponent);
    component = fixture.componentInstance;
    userService = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
    orderService = TestBed.inject(OrderService) as jasmine.SpyObj<OrderService>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize user data on init', () => {
    const user: User = {
      userId: '1',
      name: 'John Doe',
      email: 'john.doe@example.com',
      phoneNumber: '1234567890',
      address: '123 Main St',
      password: 'password123'
    };
    userService.getUserById.and.returnValue(of(user));

    currentUserSubject.next('1');
    fixture.detectChanges();

    expect(component.userData).toEqual(user);
    expect(component.editedData).toEqual(user);
    expect(component.loading).toBeFalse();
  });

  it('should handle error when loading user data', () => {
    userService.getUserById.and.returnValue(throwError({ message: 'Error loading user data' }));

    currentUserSubject.next('1');
    fixture.detectChanges();

    expect(component.error).toBe('Failed to load user data: Error loading user data');
    expect(component.loading).toBeFalse();
  });
});