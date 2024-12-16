import { Injectable } from '@angular/core';
import { AbstractControl, ValidationErrors } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class PasswordValidatorService {
  validatePassword(control: AbstractControl): ValidationErrors | null {
    const password = control.value;
    
    if (!password) {
      return null; 
    }

    const errors: ValidationErrors = {};
    
    if (password.length < 8) {
      errors['minlength'] = { requiredLength: 8, actualLength: password.length };
    }

    if (!/[A-Z]/.test(password)) {
      errors['uppercase'] = true;
    }
    
    if (!/[a-z]/.test(password)) {
      errors['lowercase'] = true;
    }
    
    if (!/[0-9]/.test(password)) {
      errors['number'] = true;
    }
    
    if (!/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
      errors['specialChar'] = true;
    }

    // Prevent common SQL injection patterns
    const sqlInjectionPatterns = [
      'OR',
      'AND',
      'UNION',
      'SELECT',
      'INSERT',
      'DELETE',
      'UPDATE',
      '--',
      ';',
      '1=1',
      '1 = 1'
    ];

    const containsSqlInjection = sqlInjectionPatterns.some(pattern => 
      password.toUpperCase().includes(pattern)
    );

    if (containsSqlInjection) {
      errors['invalidCharacters'] = true;
    }

    return Object.keys(errors).length > 0 ? errors : null;
  }
}