import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthAdminService } from '@services/auth/auth-admin.service';
import { AuthConstants } from '@constants/auth.constant';

@Injectable()
export class CheckTokenInterceptor implements HttpInterceptor {
    constructor(
        private authService: AuthAdminService,
        public jwtHelper: JwtHelperService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = localStorage.getItem(AuthConstants.TOKEN);
        if (token) {
            try {
                const decodedToken = this.jwtHelper.decodeToken(token);
                const currentTime = Date.now() / 1000;
                decodedToken.exp > currentTime ? console.log()
                    : this.authService.logout();
                return next.handle(request);
            } catch (error) {
                this.authService.logout();
                return next.handle(request);
            }
        } else {
            this.authService.logout();
            return next.handle(request);
        }
    }
}