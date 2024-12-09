// guards/auth.guard.ts
import { Injectable } from '@angular/core';
import { Router, UrlTree, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {
  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot): boolean | UrlTree {
    if (route.routeConfig?.path === 'products' || route.routeConfig?.path === 'products/:id') {
      return true;
    }

    if (this.userService.isAuthenticated) {
      return true;
    }
    return this.router.createUrlTree(['/login']);
  }
}