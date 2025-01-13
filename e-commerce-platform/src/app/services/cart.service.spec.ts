import { TestBed } from '@angular/core/testing';
import { CartService } from './cart.service';
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject } from 'rxjs';

describe('CartService', () => {
  let service: CartService;
  let platformId: Object;

  beforeEach(() => {
    platformId = 'browser'; // Mock platform ID as 'browser'

    TestBed.configureTestingModule({
      providers: [
        CartService,
        { provide: PLATFORM_ID, useValue: platformId }
      ]
    });

    service = TestBed.inject(CartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize cart count to 0 if no cart data in localStorage', () => {
    spyOn(localStorage, 'getItem').and.returnValue(null);
    service.updateCartCount();
    service.cartCount$.subscribe(count => {
      expect(count).toBe(0);
    });
  });

  it('should initialize cart count based on localStorage data', () => {
    const cartItems = JSON.stringify([{ quantity: 2 }, { quantity: 3 }]);
    spyOn(localStorage, 'getItem').and.returnValue(cartItems);
    service.updateCartCount();
    service.cartCount$.subscribe(count => {
      expect(count).toBe(5);
    });
  });

  it('should update cart count when localStorage data changes', () => {
    const cartItems = JSON.stringify([{ quantity: 1 }, { quantity: 4 }]);
    spyOn(localStorage, 'getItem').and.returnValue(cartItems);
    service.updateCartCount();
    service.cartCount$.subscribe(count => {
      expect(count).toBe(5);
    });

    const newCartItems = JSON.stringify([{ quantity: 2 }, { quantity: 3 }]);
    spyOn(localStorage, 'getItem').and.returnValue(newCartItems);
    service.updateCartCount();
    service.cartCount$.subscribe(count => {
      expect(count).toBe(5);
    });
  });

  it('should not update cart count if not running in browser', () => {
    platformId = 'server'; // Mock platform ID as 'server'
    TestBed.overrideProvider(PLATFORM_ID, { useValue: platformId });
    service = TestBed.inject(CartService);

    spyOn(localStorage, 'getItem');
    service.updateCartCount();
    expect(localStorage.getItem).not.toHaveBeenCalled();
  });
});