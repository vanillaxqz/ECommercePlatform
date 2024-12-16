import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { PasswordValidatorService } from '../../services/password-validator.service';
import { PasswordStrengthComponent } from '../password-strenght/password-strenght.component';

@Component({
  selector: 'app-user-reset-passwd',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PasswordStrengthComponent],
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
    private userService: UserService,
    private passwordValidator: PasswordValidatorService
  ) {
    this.resetForm = this.fb.group({
      password: ['', [
        Validators.required,
        this.passwordValidator.validatePassword.bind(this.passwordValidator)
      ]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
      if (!this.token) {
        this.errorMessage = 'Invalid or missing reset token';
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

      this.userService.resetPassword(
        this.token,
        this.resetForm.get('password')?.value
      ).subscribe({
        next: () => {
          this.isLoading = false;
          this.successMessage = 'Password successfully reset. Redirecting to login...';
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 2000);
        },
        error: (error) => {
          this.isLoading = false;
          this.errorMessage = error.message;
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