import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class NavbarComponent implements OnInit, OnDestroy {
  isMenuOpen = false;
  logoutMessage = false;
  cartItemCount = 0;
  private cartSubscription!: Subscription;

  constructor(
    public userService: UserService,
    private router: Router,
    private cartService: CartService
  ) {}

  ngOnInit() {
    this.cartSubscription = this.cartService.cartCount$.subscribe(
      count => this.cartItemCount = count
    );
  }

  ngOnDestroy() {
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
  }

  updateCartCount() {
    const cartData = localStorage.getItem('cartItems');
    if (cartData) {
      const cartItems = JSON.parse(cartData);
      this.cartItemCount = cartItems.reduce((total: number, item: any) => total + item.quantity, 0);
    } else {
      this.cartItemCount = 0;
    }
  }

  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }

  closeMenu(): void {
    this.isMenuOpen = false;
  }
  logout(): void {
    this.closeMenu();
    this.userService.logout();
    this.router.navigate(['/products']);

    this.logoutMessage = true;
    setTimeout(() => {
      this.logoutMessage = false;
    }, 3000);
    this.closeMenu();
  }
}