import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { PasswordValidatorService } from '../../services/password-validator.service';
import { PasswordStrengthComponent } from '../password-strenght/password-strenght.component';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PasswordStrengthComponent]
})
export class UserRegisterComponent {
  registerForm: FormGroup;
  loading = false;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private router: Router,
    private passwordValidator: PasswordValidatorService
  ) {
    this.registerForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        this.passwordValidator.validatePassword.bind(this.passwordValidator)
      ]],
      confirmPassword: ['', Validators.required],
      address: ['', [Validators.required, Validators.minLength(5)]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(group: FormGroup) {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  get passwordControl() {
    return this.registerForm.get('password');
  }

  get confirmPasswordControl() {
    return this.registerForm.get('confirmPassword');
  }

  get nameControl() {
    return this.registerForm.get('name');
  }

  get emailControl() {
    return this.registerForm.get('email');
  }

  get addressControl() {
    return this.registerForm.get('address');
  }

  get phoneNumberControl() {
    return this.registerForm.get('phoneNumber');
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = '';

    // Remove confirmPassword before sending to API
    const registrationData = {
      name: this.registerForm.get('name')?.value,
      email: this.registerForm.get('email')?.value,
      password: this.registerForm.get('password')?.value,
      address: this.registerForm.get('address')?.value,
      phoneNumber: this.registerForm.get('phoneNumber')?.value
    };

    this.userService.register(registrationData).subscribe({
      next: () => {
        this.router.navigate(['/products']);
      },
      error: error => {
        this.loading = false;
        
        if (error.status === 400) {
          if (error.error?.errors) {
            const errorMessages = [];
            const errors = error.error.errors;
            
            for (const key in errors) {
              const message = errors[key];
              switch (key.toLowerCase()) {
                case 'name':
                  errorMessages.push('Name is invalid');
                  break;
                case 'email':
                  if (message.includes('already exists')) {
                    errorMessages.push('This email is already registered');
                  } else {
                    errorMessages.push('Please enter a valid email');
                  }
                  break;
                case 'password':
                  errorMessages.push('Password must be at least 8 characters');
                  break;
                case 'phonenumber':
                  errorMessages.push('Please enter a valid 10-digit phone number');
                  break;
                case 'address':
                  errorMessages.push('Please enter a valid address');
                  break;
                default:
                  errorMessages.push(message);
              }
            }
            this.error = errorMessages.join('. ');
          } else {
            this.error = 'Please check your registration details';
          }
        } else if (error.status === 0) {
          this.error = 'Unable to connect to server. Please check your internet connection';
        } else if (error.status === 409) {
          this.error = 'This email address is already registered';
        } else {
          this.error = 'Registration failed. Please try again later';
        }
      }
    });
  }
}