import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { UserService } from '../services/user.service';
import { RouterTestingModule } from '@angular/router/testing';
import { UrlTree } from '@angular/router';
import { ActivatedRouteSnapshot, Route } from '@angular/router';

describe('AuthGuard', () => {
  let authGuard: AuthGuard;
  let userService: jasmine.SpyObj<UserService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(() => {
    const userServiceSpy = jasmine.createSpyObj('UserService', [], {
      isAuthenticated: false
    });
    const routerSpy = jasmine.createSpyObj('Router', ['createUrlTree']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        AuthGuard,
        { provide: UserService, useValue: userServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    });

    authGuard = TestBed.inject(AuthGuard);
    userService = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should be created', () => {
    expect(authGuard).toBeTruthy();
  });

  it('should allow access to products route', () => {
    const route = new ActivatedRouteSnapshot();
    Object.defineProperty(route, 'routeConfig', { value: { path: 'products' } });

    expect(authGuard.canActivate(route)).toBeTrue();
  });

  it('should allow access to products/:id route', () => {
    const route = new ActivatedRouteSnapshot();
    Object.defineProperty(route, 'routeConfig', { value: { path: 'products/:id' } });

    expect(authGuard.canActivate(route)).toBeTrue();
  });

  it('should allow access to cart route', () => {
    const route = new ActivatedRouteSnapshot();
    Object.defineProperty(route, 'routeConfig', { value: { path: 'cart' } });

    expect(authGuard.canActivate(route)).toBeTrue();
  });

  it('should allow access if user is authenticated', () => {
    Object.defineProperty(userService, 'isAuthenticated', { value: true });
    const route = new ActivatedRouteSnapshot();

    expect(authGuard.canActivate(route)).toBeTrue();
  });

  it('should redirect to login if user is not authenticated', () => {
    Object.defineProperty(userService, 'isAuthenticated', { value: false });
    const route = new ActivatedRouteSnapshot();
    const urlTree = new UrlTree();
    router.createUrlTree.and.returnValue(urlTree);

    expect(authGuard.canActivate(route)).toBe(urlTree);
    expect(router.createUrlTree).toHaveBeenCalledWith(['/login']);
  });
});