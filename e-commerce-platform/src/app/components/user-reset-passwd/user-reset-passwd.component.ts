import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-user-reset-passwd',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-reset-passwd.component.html',
  styleUrl: './user-reset-passwd.component.css'
})
export class UserResetPasswdComponent implements OnInit {
  resetForm: FormGroup;
  token: string | null = null;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) {
    this.resetForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit() {
    // Get token from URL query parameters
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
      if (!this.token) {
        this.errorMessage = 'Invalid or missing reset token';
        // Optionally redirect to home page after a delay
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 3000);
      }
    });
  }

  passwordMatchValidator(group: FormGroup) {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  onSubmit() {
    if (this.resetForm.valid && this.token) {
      this.isLoading = true;
      this.errorMessage = '';
      this.successMessage = '';

      const payload = {
        token: this.token,
        newPassword: this.resetForm.get('password')?.value
      };

      this.http.post('/api/reset-password', payload)
        .subscribe({
          next: (response) => {
            this.isLoading = false;
            this.successMessage = 'Password successfully reset. Redirecting to login...';
            setTimeout(() => {
              this.router.navigate(['/login']);
            }, 2000);
          },
          error: (error) => {
            this.isLoading = false;
            this.errorMessage = error.error?.message || 'Failed to reset password. Please try again.';
          }
        });
    }
  }

  get passwordControl() {
    return this.resetForm.get('password');
  }

  get confirmPasswordControl() {
    return this.resetForm.get('confirmPassword');
  }
}