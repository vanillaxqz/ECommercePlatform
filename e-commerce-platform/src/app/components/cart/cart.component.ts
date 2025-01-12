import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { Product } from '../../models/product.model';
import { CartService } from '../../services/cart.service';

interface CartItem extends Product {
  quantity: number;
}

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  isLoading = false;
  error: string | null = null;
  
  constructor(private cartService:CartService, private router: Router) {}

  ngOnInit() {
    this.loadCartItems();
  }

  loadCartItems() {
    this.isLoading = true;
    try {
      const cartData = localStorage.getItem('cartItems');
      this.cartItems = cartData ? JSON.parse(cartData) : [];
      this.isLoading = false;
    } catch (error) {
      this.error = 'Failed to load cart items';
      this.isLoading = false;
    }
  }

  updateQuantity(itemId: string, newQuantity: number) {
    const item = this.cartItems.find(item => item.productId === itemId);
    if (item && newQuantity > 0 && newQuantity <= item.stock) {
      item.quantity = newQuantity;
      this.saveCartItems();
    }
  }

  removeItem(itemId: string) {
    this.cartItems = this.cartItems.filter(item => item.productId !== itemId);
    this.saveCartItems();
  }

  saveCartItems() {
    try {
      localStorage.setItem('cartItems', JSON.stringify(this.cartItems));
      this.cartService.updateCartCount();
    } catch (error) {
      this.error = 'Failed to save cart items';
    }
  }

  getTotal(): number {
    return this.cartItems.reduce((sum, item) => sum + (item.price * item.quantity), 0);
  }

  clearCart() {
    this.cartItems = [];
    localStorage.removeItem('cartItems');
    this.cartService.updateCartCount();
  }

  checkout() {
    if (this.cartItems.length > 0) {
      this.router.navigate(['/checkout']);
    } else {
      this.error = 'Your cart is empty';
    }
  }

  // Static method to be used from other components
  static addToCart(product: Product) {
    try {
      const cartData = localStorage.getItem('cartItems');
      let cartItems: CartItem[] = cartData ? JSON.parse(cartData) : [];
      
      const existingItem = cartItems.find(item => item.productId === product.productId);
      
      if (existingItem) {
        if (existingItem.quantity < existingItem.stock) {
          existingItem.quantity++;
        }
      } else {
        cartItems.push({ ...product, quantity: 1 });
      }
      
      localStorage.setItem('cartItems', JSON.stringify(cartItems));
    } catch (error) {
      console.error('Failed to add item to cart:', error);
    }
  }
}