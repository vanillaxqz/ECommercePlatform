import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { CheckoutService } from '../../services/checkout.service';
import { OrderDetails } from '../../models/order.model';

interface Step {
  label: string;
  complete: boolean;
}

@Component({
  selector: 'app-order-confirmation',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-confirmation.component.html',
  styleUrls: ['./order-confirmation.component.css']
})
export class OrderConfirmationComponent implements OnInit {
  orderDetails: OrderDetails | null = null;
  isLoading = true;
  error: string | null = null;

  steps: Step[] = [
    { label: 'Order Placed', complete: true },
    { label: 'Confirmed', complete: true },
    { label: 'Shipped', complete: false }
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private checkoutService: CheckoutService
  ) {}

  ngOnInit(): void {
    const orderId = this.route.snapshot.paramMap.get('id');
    if (!orderId) {
      this.error = 'Order ID not found';
      this.isLoading = false;
      return;
    }

    this.loadOrderDetails(orderId);
  }

  private loadOrderDetails(orderId: string): void {
    this.checkoutService.getOrderDetails(orderId).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.orderDetails = response.data;
          this.updateStepsStatus();
        } else {
          this.error = response.errorMessage || 'Failed to load order details';
        }
        this.isLoading = false;
      },
      error: (error) => {
        this.error = 'Error loading order details. Please try again later.';
        this.isLoading = false;
        console.error('Error loading order details:', error);
      }
    });
  }

  private updateStepsStatus(): void {
    if (this.orderDetails) {
      this.steps[2].complete = this.orderDetails.status === 3;
    }
  }

  getProgressWidth(): number {
    const completedSteps = this.steps.filter(step => step.complete).length;
    return ((completedSteps - 1) / (this.steps.length - 1)) * 100;
  }

  getStepClasses(complete: boolean): string {
    return `w-10 h-10 rounded-full flex items-center justify-center border-2 transition-colors duration-200 ${
      complete ? 'border-blue-600 bg-blue-600 text-white' : 'border-gray-300 bg-white text-gray-400'
    }`;
  }

  getStatusClasses(status: number): string {
    const baseClasses = 'px-3 py-1 rounded-full text-sm font-medium';
    switch (status) {
      case 1:
        return `${baseClasses} bg-green-100 text-green-800`;
      case 2:
        return `${baseClasses} bg-yellow-100 text-yellow-800`;
      case 3:
        return `${baseClasses} bg-blue-100 text-blue-800`;
      default:
        return `${baseClasses} bg-gray-100 text-gray-800`;
    }
  }

  getStatusText(status: number): string {
    switch (status) {
      case 1:
        return 'Confirmed';
      case 2:
        return 'Processing';
      case 3:
        return 'Shipped';
      default:
        return 'Unknown';
    }
  }

  goToOrderHistory(): void {
    this.router.navigate(['/profile']);
  }

  continueShopping(): void {
    this.router.navigate(['/products']);
  }
}