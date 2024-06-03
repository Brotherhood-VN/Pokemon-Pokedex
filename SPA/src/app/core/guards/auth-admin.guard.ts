import { Injectable, inject } from '@angular/core';
import { CanMatchFn, Route, Router } from '@angular/router';
import { AuthAdminService } from '@services/auth/auth-admin.service';

@Injectable({
  providedIn: 'root'
})
export class AuthAdminGuard {
  constructor(private router: Router) { }

  canMathAuth(authService: AuthAdminService): boolean {
    if (authService.isAuthenticated())
      return true;
    else {
      this.router.navigateByUrl('auth/login');
      return false;
    }
  }
};

export const AuthGuard: CanMatchFn = (route: Route) => {
  const authService = inject(AuthAdminService);
  return inject(AuthAdminGuard).canMathAuth(authService);
}