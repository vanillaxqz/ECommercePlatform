// user-profile.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { User } from '../../models/user.model';
import { OrderService } from '../../services/order.service';
import { OrderDetails } from '../../models/order.model';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit {
  userData: User | null = null;
  loading = true;
  error: string | null = null;
  isEditing = false;
  editedData: Partial<User> = {};

  orderHistory: OrderDetails[] = [];
  orderHistoryLoading = true;
  orderHistoryError: string | null = null;

  constructor(
    private userService: UserService,
    private router: Router,
    private orderService: OrderService // Inject the OrderService
  ) {}

  ngOnInit() {
    this.userService.currentUserObservable.subscribe({
      next: (currentUser) => {
        if (typeof currentUser === 'string') {
          this.userService.getUserById(currentUser).subscribe({
            next: (user) => {
              this.userData = user;
              this.editedData = { ...user };
              this.loading = false;
              this.loadOrderHistory(user.userId); // Load order history for the user
            },
            error: (error) => {
              this.error = 'Failed to load user data: ' + error.message;
              this.loading = false;
            }
          });
        } else if (currentUser?.userId) {
          this.userData = currentUser;
          this.editedData = { ...currentUser };
          this.loading = false;
          this.loadOrderHistory(currentUser.userId); // Load order history for the user
        } else {
          this.error = 'No user ID available';
          this.userData = null;
          this.loading = false;
        }
      },
      error: (error) => {
        console.error('Error in user subscription:', error);
      }
    });
  }

  // Add this method to load order history
  loadOrderHistory(userId: string): void {
    this.orderHistoryLoading = true;
    this.orderHistoryError = null;

    this.orderService.getOrderHistory(userId).subscribe({
      next: (response) => {
        this.orderHistory = response.data;
        this.orderHistoryLoading = false;
      },
      error: (error) => {
        this.orderHistoryError = 'Failed to load order history: ' + error.message;
        this.orderHistoryLoading = false;
      }
    });
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

  getStatusClasses(status: number): string {
    switch (status) {
      case 1:
        return 'status-1'; // Confirmed
      case 2:
        return 'status-2'; // Processing
      case 3:
        return 'status-3'; // Shipped
      default:
        return ''; // Unknown
    }
  }

  startEditing(): void {
    if (this.userData) {
      this.editedData = { ...this.userData };
      this.isEditing = true;
    }
  }

  cancelEditing(): void {
    this.isEditing = false;
    this.editedData = { ...this.userData };
  }

  saveChanges(): void {
    if (this.editedData && this.userData) {
      const updateData = {
        name: this.editedData.name || '',
        email: this.editedData.email || '',
        address: this.editedData.address || '',
        phoneNumber: this.editedData.phoneNumber || ''
      };

      this.userService.editUser(updateData).subscribe({
        next: (updatedUser) => {
          this.userData = updatedUser;
          this.isEditing = false;
          // Show success message or notification here if you want
        },
        error: (error) => {
          this.error = 'Failed to update profile: ' + error.message;
        }
      });
    }
  }

  deleteAccount(): void {
    if(confirm('Are you sure you want to delete your account?')) {
      if (this.userData) {
        this.userService.deleteUser(this.userData.userId).subscribe({
          next: () => {
            this.userService.logout();
            this.router.navigate(['/products']);
          },
          error: (error) => {
            this.error = 'Failed to delete account: ' + error.message;
          }
        });
      }
    }
  }
}