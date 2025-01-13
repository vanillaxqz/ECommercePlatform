import { TestBed } from '@angular/core/testing';
import { PasswordValidatorService } from './password-validator.service';
import { FormControl } from '@angular/forms';

describe('PasswordValidatorService', () => {
  let service: PasswordValidatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PasswordValidatorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return null if password is empty', () => {
    const control = new FormControl('');
    const result = service.validatePassword(control);
    expect(result).toBeNull();
  });

  it('should validate minimum length', () => {
    const control = new FormControl('short');
    const result = service.validatePassword(control);
    expect(result).toEqual({ minlength: { requiredLength: 8, actualLength: 5 } });
  });

  it('should validate uppercase character', () => {
    const control = new FormControl('lowercase1!');
    const result = service.validatePassword(control);
    expect(result).toEqual({ uppercase: true });
  });

  it('should validate lowercase character', () => {
    const control = new FormControl('UPPERCASE1!');
    const result = service.validatePassword(control);
    expect(result).toEqual({ lowercase: true });
  });

  it('should validate number', () => {
    const control = new FormControl('NoNumber!');
    const result = service.validatePassword(control);
    expect(result).toEqual({ number: true });
  });

  it('should validate special character', () => {
    const control = new FormControl('NoSpecial1');
    const result = service.validatePassword(control);
    expect(result).toEqual({ specialChar: true });
  });

  it('should validate multiple errors', () => {
    const control = new FormControl('short');
    const result = service.validatePassword(control);
    expect(result).toEqual({
      minlength: { requiredLength: 8, actualLength: 5 },
      uppercase: true,
      number: true,
      specialChar: true
    });
  });

  it('should pass validation for a strong password', () => {
    const control = new FormControl('StrongPass1!');
    const result = service.validatePassword(control);
    expect(result).toBeNull();
  });
});