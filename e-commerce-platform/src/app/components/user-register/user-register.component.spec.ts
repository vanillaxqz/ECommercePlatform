import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { UserRegisterComponent } from './user-register.component';
import { UserService } from '../../services/user.service';
import { PasswordValidatorService } from '../../services/password-validator.service';
import { PasswordStrengthComponent } from '../password-strenght/password-strenght.component';
import { of, throwError } from 'rxjs';
import { Router } from '@angular/router';

describe('UserRegisterComponent', () => {
  let component: UserRegisterComponent;
  let fixture: ComponentFixture<UserRegisterComponent>;
  let userService: jasmine.SpyObj<UserService>;
  let passwordValidator: jasmine.SpyObj<PasswordValidatorService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const userServiceSpy = jasmine.createSpyObj('UserService', ['register']);
    const passwordValidatorSpy = jasmine.createSpyObj('PasswordValidatorService', ['validatePassword']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [UserRegisterComponent, PasswordStrengthComponent],
      imports: [ReactiveFormsModule, RouterTestingModule],
      providers: [
        { provide: UserService, useValue: userServiceSpy },
        { provide: PasswordValidatorService, useValue: passwordValidatorSpy },
        { provide: Router, useValue: routerSpy }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserRegisterComponent);
    component = fixture.componentInstance;
    userService = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
    passwordValidator = TestBed.inject(PasswordValidatorService) as jasmine.SpyObj<PasswordValidatorService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the register form', () => {
    expect(component.registerForm).toBeDefined();
    expect(component.registerForm.controls['name']).toBeDefined();
    expect(component.registerForm.controls['email']).toBeDefined();
    expect(component.registerForm.controls['password']).toBeDefined();
    expect(component.registerForm.controls['confirmPassword']).toBeDefined();
    expect(component.registerForm.controls['address']).toBeDefined();
    expect(component.registerForm.controls['phoneNumber']).toBeDefined();
  });

  it('should validate password match', () => {
    component.registerForm.setValue({
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      confirmPassword: 'password1234',
      address: '123 Main St',
      phoneNumber: '1234567890'
    });
    expect(component.registerForm.errors).toEqual({ passwordMismatch: true });

    component.registerForm.setValue({
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      confirmPassword: 'password123',
      address: '123 Main St',
      phoneNumber: '1234567890'
    });
    expect(component.registerForm.errors).toBeNull();
  });

  it('should handle form submission', () => {
    component.registerForm.setValue({
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      confirmPassword: 'password123',
      address: '123 Main St',
      phoneNumber: '1234567890'
    });
    userService.register.and.returnValue(of({ userId: '1', name: 'John Doe', email: 'john.doe@example.com', password: 'password123', phoneNumber: '1234567890', address: '123 Main St' }));

    component.onSubmit();

    expect(component.loading).toBeTrue();
    expect(userService.register).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should handle registration error', () => {
    component.registerForm.setValue({
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      confirmPassword: 'password123',
      address: '123 Main St',
      phoneNumber: '1234567890'
    });
    userService.register.and.returnValue(throwError({ error: { message: 'Registration failed' } }));

    component.onSubmit();

    expect(component.loading).toBeFalse();
    expect(component.error).toBe('Registration failed');
  });
});