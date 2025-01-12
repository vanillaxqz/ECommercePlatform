import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { User } from '../../models/user.model';

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

  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit() {
    this.userService.currentUserObservable.subscribe({
      next: (currentUser) => {
        if (typeof currentUser === 'string') {
          this.userService.getUserById(currentUser).subscribe({
            next: (user) => {
              console.log('Fetched user details:', user);
              this.userData = user;
              this.editedData = { ...user };
              this.loading = false;
            },
            error: (error) => {
              console.error('Error fetching user details:', error);
              this.error = 'Failed to load user data: ' + error.message;
              this.loading = false;
            }
          });
        } else if (currentUser?.userId) {
          console.log('Fetching user details for complete user object:', currentUser);
          this.userData = currentUser;
          this.editedData = { ...currentUser };
          this.loading = false;
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
        password: this.editedData.password || '',
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
    if (confirm('Are you sure you want to delete your account? This action cannot be undone.')) {
      // Add actual delete account logic here
      this.userService.logout();
      this.router.navigate(['/login']);
    }
  }
}