import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CartComponent } from './cart.component';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

describe('CartComponent', () => {
  let component: CartComponent;
  let fixture: ComponentFixture<CartComponent>;
  let cartService: jasmine.SpyObj<CartService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const cartServiceSpy = jasmine.createSpyObj('CartService', ['updateCartCount']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [CartComponent],
      providers: [
        { provide: CartService, useValue: cartServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CartComponent);
    component = fixture.componentInstance;
    cartService = TestBed.inject(CartService) as jasmine.SpyObj<CartService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should update item quantity', () => {
    component.cartItems = [{
      productId: '1', name: 'Product 1', price: 100, quantity: 1, stock: 10,
      description: '',
      category: 0,
      userId: ''
    }];
    component.updateQuantity('1', 5);
    expect(component.cartItems[0].quantity).toBe(5);
  });

  it('should remove item from cart', () => {
    component.cartItems = [{
      productId: '1', name: 'Product 1', price: 100, quantity: 1, stock: 10,
      description: '',
      category: 0,
      userId: ''
    }];
    component.removeItem('1');
    expect(component.cartItems.length).toBe(0);
  });

  it('should save cart items', () => {
    spyOn(localStorage, 'setItem');
    component.cartItems = [{
      productId: '1', name: 'Product 1', price: 100, quantity: 1, stock: 10,
      description: '',
      category: 0,
      userId: ''
    }];
    component.saveCartItems();
    expect(localStorage.setItem).toHaveBeenCalledWith('cartItems', JSON.stringify(component.cartItems));
    expect(cartService.updateCartCount).toHaveBeenCalled();
  });

  it('should calculate total price correctly', () => {
    component.cartItems = [
      {
        productId: '1', name: 'Product 1', price: 100, quantity: 2, stock: 10,
        description: '',
        category: 0,
        userId: ''
      },
      {
        productId: '2', name: 'Product 2', price: 50, quantity: 1, stock: 10,
        description: '',
        category: 0,
        userId: ''
      }
    ];
    expect(component.getTotal()).toBe(250);
  });

  it('should clear the cart', () => {
    spyOn(localStorage, 'removeItem');
    component.cartItems = [{
      productId: '1', name: 'Product 1', price: 100, quantity: 1, stock: 10,
      description: '',
      category: 0,
      userId: ''
    }];
    component.clearCart();
    expect(component.cartItems.length).toBe(0);
    expect(localStorage.removeItem).toHaveBeenCalledWith('cartItems');
    expect(cartService.updateCartCount).toHaveBeenCalled();
  });

  it('should navigate to checkout on checkout button click', () => {
    component.cartItems = [{
      productId: '1', name: 'Product 1', price: 100, quantity: 1, stock: 10,
      description: '',
      category: 0,
      userId: ''
    }];
    component.checkout();
    expect(router.navigate).toHaveBeenCalledWith(['/checkout']);
  });

  it('should handle empty cart on checkout', () => {
    component.cartItems = [];
    component.checkout();
    expect(component.error).toBe('Your cart is empty');
  });

  it('should handle error when saving cart items', () => {
    spyOn(localStorage, 'setItem').and.throwError('Error saving to localStorage');
    component.saveCartItems();
    expect(component.error).toBe('Failed to save cart items');
  });

  it('should add item to cart statically', () => {
    spyOn(localStorage, 'setItem');
    const product = { productId: '1', name: 'Product 1', price: 100, quantity: 1, stock: 10 };
    component.cartItems = [];
    component.cartItems.push({
      productId: '1',
      name: 'Product 1',
      price: 100,
      quantity: 1,
      stock: 10,
      description: '',
      category: 0,
      userId: ''
    });
    component.saveCartItems();
    const cartItems = JSON.parse(localStorage.getItem('cartItems') || '[]');
    expect(cartItems.length).toBe(1);
    expect(cartItems[0].name).toBe('Product 1');
  });
});