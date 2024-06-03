import { Component } from '@angular/core';
import { CaptionConstants, MessageConstants } from '@constants/message.constant';
import { UserForLoginParam } from '@models/auth';
import { AuthAdminService } from '@services/auth/auth-admin.service';
import { InjectBase } from '@utilities/inject-base';
import { first } from 'rxjs';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styles: [
        `
            :host ::ng-deep .pi-eye,
            :host ::ng-deep .pi-eye-slash {
                transform: scale(1.6);
                margin-right: 1rem;
                color: var(--primary-color) !important;
            }
        `,
    ],
})
export class LoginComponent extends InjectBase {
    params: UserForLoginParam = <UserForLoginParam>{};

    constructor(
        public layoutService: LayoutService,
        private _authService: AuthAdminService
    ) {
        super();
    }

    onSubmit() {
        this._authService
            .login(this.params)
            .pipe(first())
            .subscribe({
                next: () => {
                    this._toast.success(MessageConstants.LOGIN_OK, CaptionConstants.SUCCESS);
                    this._router.navigate(['/admin']);
                },
                error: () => {
                    this._toast.error('Đăng nhập thất bại', CaptionConstants.ERROR);
                },
            });
    }

    forgotPassword() {}
}
