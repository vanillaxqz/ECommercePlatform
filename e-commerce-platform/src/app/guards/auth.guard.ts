// guards/auth.guard.ts
import { Injectable } from '@angular/core';
import { Router, UrlTree } from '@angular/router';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {
  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  canActivate(): boolean | UrlTree {
    if (this.userService.isAuthenticated) {
      return true;
    }
    return this.router.createUrlTree(['/login']);
  }
}