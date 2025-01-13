import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { CheckoutComponent } from './checkout.component';
import { CheckoutService } from '../../services/checkout.service';
import { CartService } from '../../services/cart.service';
import { UserService } from '../../services/user.service';
import { of } from 'rxjs';

describe('CheckoutComponent', () => {
  let component: CheckoutComponent;
  let fixture: ComponentFixture<CheckoutComponent>;
  let checkoutService: jasmine.SpyObj<CheckoutService>;
  let cartService: jasmine.SpyObj<CartService>;
  let userService: jasmine.SpyObj<UserService>;

  beforeEach(async () => {
    const checkoutServiceSpy = jasmine.createSpyObj('CheckoutService', ['']);
    const cartServiceSpy = jasmine.createSpyObj('CartService', ['']);
    const userServiceSpy = jasmine.createSpyObj('UserService', ['getUserProfile']);
    userServiceSpy.getUserProfile.and.returnValue(of({
      name: 'John Doe',
      email: 'john.doe@example.com',
      phoneNumber: '1234567890',
      address: '123 Main St'
    }));

    await TestBed.configureTestingModule({
      declarations: [CheckoutComponent],
      imports: [FormsModule, RouterTestingModule],
      providers: [
        { provide: CheckoutService, useValue: checkoutServiceSpy },
        { provide: CartService, useValue: cartServiceSpy },
        { provide: UserService, useValue: userServiceSpy }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CheckoutComponent);
    component = fixture.componentInstance;
    checkoutService = TestBed.inject(CheckoutService) as jasmine.SpyObj<CheckoutService>;
    cartService = TestBed.inject(CartService) as jasmine.SpyObj<CartService>;
    userService = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have default card details and validation errors', () => {
    expect(component.cardName).toBe('');
    expect(component.cardNumber).toBe('');
    expect(component.expiryDate).toBe('');
    expect(component.cvv).toBe('');
    expect(component.validationErrors).toEqual({
      cardNumber: [],
      expiry: [],
      cvv: [],
      name: []
    });
  });

  it('should have default delivery option and new user details', () => {
    expect(component.deliveryOption).toBe('saved');
    expect(component.newName).toBe('');
    expect(component.newEmail).toBe('');
    expect(component.newPhoneNumber).toBe('');
    expect(component.newAddress).toBe('');
    expect(component.newPostalCode).toBe('');
  });
});