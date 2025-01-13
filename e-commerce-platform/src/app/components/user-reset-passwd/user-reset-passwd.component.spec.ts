import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { UserResetPasswdComponent } from './user-reset-passwd.component';
import { UserService } from '../../services/user.service';
import { of, throwError } from 'rxjs';
import { Router } from '@angular/router';

describe('UserResetPasswdComponent', () => {
  let component: UserResetPasswdComponent;
  let fixture: ComponentFixture<UserResetPasswdComponent>;
  let userService: jasmine.SpyObj<UserService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const userServiceSpy = jasmine.createSpyObj('UserService', ['resetPassword']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [UserResetPasswdComponent],
      imports: [ReactiveFormsModule, RouterTestingModule],
      providers: [
        { provide: UserService, useValue: userServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserResetPasswdComponent);
    component = fixture.componentInstance;
    userService = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the reset form', () => {
    expect(component.resetForm).toBeDefined();
    expect(component.resetForm.controls['password']).toBeDefined();
    expect(component.resetForm.controls['confirmPassword']).toBeDefined();
  });

  it('should validate password match', () => {
    component.resetForm.setValue({
      password: 'password123',
      confirmPassword: 'password1234'
    });
    expect(component.resetForm.errors).toEqual({ passwordMismatch: true });

    component.resetForm.setValue({
      password: 'password123',
      confirmPassword: 'password123'
    });
    expect(component.resetForm.errors).toBeNull();
  });

  it('should handle form submission successfully', () => {
    component.resetForm.setValue({
      password: 'password123',
      confirmPassword: 'password123'
    });
    component.token = 'valid-token';
    userService.resetPassword.and.returnValue(of({}));

    component.onSubmit();

    expect(component.isLoading).toBeTrue();
    expect(userService.resetPassword).toHaveBeenCalledWith('valid-token', 'password123');
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should handle form submission error', () => {
    component.resetForm.setValue({
      password: 'password123',
      confirmPassword: 'password123'
    });
    component.token = 'valid-token';
    userService.resetPassword.and.returnValue(throwError({ message: 'Reset failed' }));

    component.onSubmit();

    expect(component.isLoading).toBeFalse();
    expect(component.errorMessage).toBe('Reset failed');
  });
});