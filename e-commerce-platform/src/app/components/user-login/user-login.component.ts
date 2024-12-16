import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule]
})
export class UserLoginComponent implements OnInit {
  loginForm: FormGroup;
  resetForm: FormGroup;
  loading = false;
  error = '';
  showResetModal = false;
  resetLoading = false;
  resetError = '';
  resetSuccess = false;

  constructor(
    private formBuilder: FormBuilder,
    public userService: UserService,
    private router: Router
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });

    this.resetForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void {
    if (this.userService.isAuthenticated) {
      this.router.navigate(['/products']);
    }
  }

  continueAsGuest(): void {
    this.userService.continueAsGuest();
    this.router.navigate(['/products']);
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = '';

    const { email, password } = this.loginForm.value;

    this.userService.login(email, password).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/products']);
      },
      error: error => {
        this.loading = false;
        this.error = error.message;
      }
    });
  }

  openResetModal() {
    this.showResetModal = true;
    this.resetForm.reset();
    this.resetError = '';
    this.resetSuccess = false;
  }

  closeResetModal() {
    this.showResetModal = false;
    setTimeout(() => {
      this.resetSuccess = false;
      this.resetError = '';
    }, 300);
  }

  onResetSubmit() {
    if (this.resetForm.valid && !this.resetLoading) {
      this.resetLoading = true;
      this.resetError = '';

      this.userService.requestPasswordReset(this.resetForm.get('email')?.value)
        .subscribe({
          next: () => {
            this.resetLoading = false;
            this.resetSuccess = true;
            setTimeout(() => {
              this.closeResetModal();
            }, 2000);
          },
          error: (error) => {
            this.resetLoading = false;
            this.resetError = error.message;
          }
        });
    }
  }
}