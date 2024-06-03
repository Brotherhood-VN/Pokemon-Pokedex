import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthConstants } from '@constants/auth.constant';
import { environment } from '@env/environment';
import { Auth, ResetPassword, TokenRequestParam, UserForLoginParam, UserReturned } from '@models/auth';
import { OperationResult } from '@utilities/operation-result';
import { Observable, map, of, tap } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AuthAdminService {
  apiUrl: string = `${environment.apiUrl}/Auth`;
  decodedToken: any;
  currentUser: Auth = <Auth>{};
  refreshTokenTimeout: any;
  constructor(
    public jwtHelper: JwtHelperService,
    private http: HttpClient,
    private router: Router) { }

  login(val: UserForLoginParam) {
    return this.http.post<UserReturned>(this.apiUrl + '/Login', val).pipe(
      tap(res => {
        if (res) {
          if (val.rememberMe) {
            localStorage.setItem(AuthConstants.REFRESH_TOKEN, res.refreshToken);
          }
          localStorage.setItem(AuthConstants.TOKEN, res.token);
          localStorage.setItem(AuthConstants.USER_INFO, JSON.stringify(res.user));
          localStorage.setItem(AuthConstants.MENUS, JSON.stringify(res.menus));

          this.decodedToken = this.jwtHelper.decodeToken(res.token);
          this.currentUser = res.user;

          this.startRefreshTokenTimer();
        }
      }),
    );
  }

  logout(): void {
    let tokenRequest: TokenRequestParam = { token: localStorage.getItem(AuthConstants.REFRESH_TOKEN) } as TokenRequestParam;
    if (tokenRequest.token !== null)
      this.http.post<any>(`${this.apiUrl}/RevokeToken`, tokenRequest);

    localStorage.removeItem(AuthConstants.TOKEN);
    localStorage.removeItem(AuthConstants.REFRESH_TOKEN);
    localStorage.removeItem(AuthConstants.USER_INFO);
    localStorage.removeItem(AuthConstants.MENUS);

    this.stopRefreshTokenTimer();
    this.decodedToken = null;

    this.router.navigate(['/auth/login']);
  }

  generateCodeResetPass(model: ResetPassword): Observable<OperationResult> {
    return this.http.post<OperationResult>(this.apiUrl + '/GenerateCodeResetPass', model);
  }

  changePassword(model: ResetPassword): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.apiUrl + '/ChangePassword', model);
  }

  get userInfo() {
    return JSON.parse(localStorage.getItem(AuthConstants.USER_INFO) ?? "");
  }

  isAuthenticated(): boolean {
    const token: string = localStorage.getItem(AuthConstants.TOKEN);
    const curentUser: Auth = JSON.parse(localStorage.getItem(AuthConstants.USER_INFO));
    if (!curentUser || !token) {
      return false;
    }
    return !this.jwtHelper.isTokenExpired(token);
  }

  refreshToken() {
    let refreshToken: string = localStorage.getItem(AuthConstants.REFRESH_TOKEN);

    if (refreshToken) {
      let tokenRequest: TokenRequestParam = <TokenRequestParam>{ token: refreshToken };
      return this.http.post<UserReturned>(`${this.apiUrl}/RefreshToken`, tokenRequest)
        .pipe(
          map((res) => {
            localStorage.setItem(AuthConstants.REFRESH_TOKEN, res.refreshToken);
            localStorage.setItem(AuthConstants.TOKEN, res.token);
            localStorage.setItem(AuthConstants.USER_INFO, JSON.stringify(res.user));
            localStorage.setItem(AuthConstants.MENUS, JSON.stringify(res.menus));

            this.decodedToken = this.jwtHelper.decodeToken(res.token);
            this.currentUser = res.user;

            this.startRefreshTokenTimer();

            return res;
          }));
    } else {
      return of(null);
    }
  }

  private startRefreshTokenTimer() {
    // Gán timeout để làm mới token trước 1 phút nó hết hạn
    const expires = new Date(this.decodedToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - (60 * 1000);
    this.refreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), timeout);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
