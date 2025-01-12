import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { Router } from '@angular/router';
import { CheckoutService } from '../../services/checkout.service';
import { CartService } from '../../services/cart.service';

interface CardValidation {
  cardNumber: string[];
  expiry: string[];
  cvv: string[];
  name: string[];
}

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [FormsModule, CommonModule],
  providers: [CheckoutService],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css'],
})
export class CheckoutComponent implements OnInit {
  // Card details
  cardName: string = '';
  cardNumber: string = '';
  expiryDate: string = '';
  cvv: string = '';

  // Validation errors
  validationErrors: CardValidation = {
    cardNumber: [],
    expiry: [],
    cvv: [],
    name: []
  };

  // Component properties
  deliveryOption: string = 'saved';
  newName: string = '';
  newEmail: string = '';
  newPhoneNumber: string = '';
  newAddress: string = '';
  newPostalCode: string = '';
  isLoading: boolean = true;
  userProfile: User | null = null;
  cartItems: any[] = [];
  cartSubtotal: number = 0;
  cartTotal: number = 0;
  shippingCost: number = 0;
  error: string | null = null;

  constructor(
    private userService: UserService,
    private checkoutService: CheckoutService,
    private router: Router,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.userService.currentUserObservable.subscribe({
      next: (currentUser) => {
        if (typeof currentUser === 'string') {
          this.userService.getUserById(currentUser).subscribe({
            next: (user) => {
              this.userProfile = user;
              this.loadCartAndCalculateTotals();
              this.isLoading = false;
            },
            error: (error) => {
              this.error = 'Failed to load user data: ' + error.message;
              this.isLoading = false;
            },
          });
        } else if (currentUser?.userId) {
          this.userProfile = currentUser;
          this.loadCartAndCalculateTotals();
          this.isLoading = false;
        } else {
          this.error = 'No user ID available';
          this.isLoading = false;
        }
      },
      error: (error) => {
        this.error = 'Error in user subscription: ' + error.message;
        this.isLoading = false;
      },
    });
  }

  // Card type detection methods
isVisa(cardNumber: string): boolean {
  const cleaned = cardNumber.replace(/\D/g, '');
  return cleaned.startsWith('4');
}

isMastercard(cardNumber: string): boolean {
  const cleaned = cardNumber.replace(/\D/g, '');
  return /^5[1-5]/.test(cleaned);
}

isAmex(cardNumber: string): boolean {
  const cleaned = cardNumber.replace(/\D/g, '');
  return /^3[47]/.test(cleaned);
}

  private loadCartAndCalculateTotals(): void {
    try {
      this.cartItems = JSON.parse(localStorage.getItem('cartItems') || '[]');
      this.calculateTotals();
    } catch (error) {
      this.error = 'Error loading cart items';
      this.cartItems = [];
    }
  }

  calculateTotals(): void {
    this.cartSubtotal = this.cartItems.reduce(
      (total, item) => total + item.price * item.quantity,
      0
    );
    this.shippingCost = this.cartSubtotal >= 20 ? 0 : 10;
    this.cartTotal = this.cartSubtotal + this.shippingCost;
  }

  // Validation methods
  private validateCardNumber(cardNumber: string): string[] {
    const errors: string[] = [];
    // Remove spaces, hyphens, and any non-digit characters
    const cleaned = cardNumber.replace(/[\s\-]/g, '').replace(/\D/g, '');

    if (!cleaned) {
      errors.push('Card number is required');
      return errors;
    }

    if (cleaned.length < 13 || cleaned.length > 19) {
      errors.push('Card number must be between 13 and 19 digits');
    }

    if (!this.luhnCheck(cleaned)) {
      errors.push('Invalid card number');
    }

    return errors;
  }

  private luhnCheck(cardNumber: string): boolean {
    let sum = 0;
    let isEven = false;

    // Loop through values starting from the rightmost side
    for (let i = cardNumber.length - 1; i >= 0; i--) {
      let digit = parseInt(cardNumber.charAt(i));

      if (isEven) {
        digit *= 2;
        if (digit > 9) {
          digit -= 9;
        }
      }

      sum += digit;
      isEven = !isEven;
    }

    return sum % 10 === 0;
  }

  private validateExpiryDate(expiry: string): string[] {
    const errors: string[] = [];
    
    if (!expiry) {
      errors.push('Expiry date is required');
      return errors;
    }

    // Check format (MM/YY)
    if (!/^\d{2}\/\d{2}$/.test(expiry)) {
      errors.push('Expiry date must be in MM/YY format');
      return errors;
    }

    const [month, year] = expiry.split('/');
    const currentDate = new Date();
    const currentYear = currentDate.getFullYear() % 100;
    const currentMonth = currentDate.getMonth() + 1;
    const expiryMonth = parseInt(month);
    const expiryYear = parseInt(year);

    if (expiryMonth < 1 || expiryMonth > 12) {
      errors.push('Invalid month');
    }

    if (expiryYear < currentYear || (expiryYear === currentYear && expiryMonth < currentMonth)) {
      errors.push('Card has expired');
    }

    return errors;
  }

  private validateCVV(cvv: string): string[] {
    const errors: string[] = [];
    
    if (!cvv) {
      errors.push('CVV is required');
      return errors;
    }

    const cleaned = cvv.replace(/\D/g, '');
    
    if (cleaned.length < 3 || cleaned.length > 4) {
      errors.push('CVV must be 3 or 4 digits');
    }

    return errors;
  }

  private validateCardName(name: string): string[] {
    const errors: string[] = [];
    
    if (!name.trim()) {
      errors.push('Name is required');
      return errors;
    }

    if (!/^[a-zA-Z\s-']+$/.test(name)) {
      errors.push('Name can only contain letters, spaces, hyphens, and apostrophes');
    }

    if (name.trim().length < 2) {
      errors.push('Name is too short');
    }

    return errors;
  }

  // Format card number as user types
  onCardNumberInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value.replace(/\D/g, '');
    
    // Format in groups of 4
    if (value.length > 0) {
      value = value.match(/.{1,4}/g)?.join(' ') || value;
    }
    
    input.value = value;
    this.cardNumber = value;
  }

  // Format expiry date as user types
  onExpiryDateInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value.replace(/\D/g, '');
    
    if (value.length >= 2) {
      value = value.substring(0, 2) + '/' + value.substring(2, 4);
    }
    
    input.value = value;
    this.expiryDate = value;
  }

  // Validate all card fields
  validateCardFields(): boolean {
    this.validationErrors = {
      cardNumber: this.validateCardNumber(this.cardNumber),
      expiry: this.validateExpiryDate(this.expiryDate),
      cvv: this.validateCVV(this.cvv),
      name: this.validateCardName(this.cardName)
    };

    return Object.values(this.validationErrors).every(errors => errors.length === 0);
  }

  submitOrder(): void {
    if (!this.validateCardFields()) {
      return;
    }

    this.isLoading = true;
    this.error = null;

    this.checkoutService.processCheckout()
      .subscribe({
        next: (orderId) => {
          this.isLoading = false;
          localStorage.removeItem('cartItems');
          this.cartService.updateCartCount();
          this.router.navigate(['/order-confirmation', orderId]);
        },
        error: (error) => {
          this.isLoading = false;
          if (error.status === 401) {
            this.error = 'Your session has expired. Please log in again';
          } else {
            this.error = error.message || 'Failed to process order. Please try again';
          }
          console.error('Error processing order:', error);
        }
      });
  }
}