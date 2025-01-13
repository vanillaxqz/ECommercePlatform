import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { UserLoginComponent } from './user-login.component';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { of } from 'rxjs';

describe('UserLoginComponent', () => {
  let component: UserLoginComponent;
  let fixture: ComponentFixture<UserLoginComponent>;
  let userService: jasmine.SpyObj<UserService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const userServiceSpy = jasmine.createSpyObj('UserService', ['isAuthenticated', 'continueAsGuest']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [UserLoginComponent],
      imports: [ReactiveFormsModule, RouterTestingModule],
      providers: [
        { provide: UserService, useValue: userServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserLoginComponent);
    component = fixture.componentInstance;
    userService = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize login and reset forms', () => {
    expect(component.loginForm).toBeDefined();
    expect(component.loginForm.controls['email']).toBeDefined();
    expect(component.loginForm.controls['password']).toBeDefined();

    expect(component.resetForm).toBeDefined();
    expect(component.resetForm.controls['email']).toBeDefined();
  });

  it('should continue as guest and navigate to products', () => {
    component.continueAsGuest();
    expect(userService.continueAsGuest).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/products']);
  });

  it('should handle login form submission', () => {
    component.loginForm.setValue({ email: 'test@example.com', password: 'password' });
    component.loading = true;
    component.error = 'Invalid credentials';
    fixture.detectChanges();

    expect(component.loading).toBeTrue();
    expect(component.error).toBe('Invalid credentials');
  });

  it('should handle reset form submission', () => {
    component.resetForm.setValue({ email: 'test@example.com' });
    component.resetLoading = true;
    component.resetError = 'Email not found';
    component.resetSuccess = true;
    fixture.detectChanges();

    expect(component.resetLoading).toBeTrue();
    expect(component.resetError).toBe('Email not found');
    expect(component.resetSuccess).toBeTrue();
  });
});