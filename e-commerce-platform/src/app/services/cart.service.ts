import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartCountSubject = new BehaviorSubject<number>(0);
  cartCount$ = this.cartCountSubject.asObservable();

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    if (isPlatformBrowser(this.platformId)) {
      this.updateCartCount();
    }
  }

  updateCartCount() {
    if (isPlatformBrowser(this.platformId)) {
      const cartData = localStorage.getItem('cartItems');
      if (cartData) {
        const cartItems = JSON.parse(cartData);
        this.cartCountSubject.next(cartItems.reduce((total: number, item: any) => total + item.quantity, 0));
      } else {
        this.cartCountSubject.next(0);
      }
    }
  }
}