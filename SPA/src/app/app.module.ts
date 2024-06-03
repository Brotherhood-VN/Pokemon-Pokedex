import { APP_INITIALIZER, NgModule } from '@angular/core';
import { LocationStrategy, PathLocationStrategy } from '@angular/common';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AppLayoutModule } from './layout/app.layout.module';
import { NotfoundComponent } from './views/notfound/notfound.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { NgxProgressHttpModule } from '@kken94/ngx-progress';
import { ScrollTopModule } from 'primeng/scrolltop';
import { JwtModule } from '@auth0/angular-jwt';
import { environment } from "@env/environment";
import { QuillModule } from 'ngx-quill';
import { ConfirmationService, MessageService } from 'primeng/api';
import { AuthAdminService } from '@services/auth/auth-admin.service';
import { RefreshTokenInitializer } from '@utilities/refresh-token.initializer';
import { CheckTokenInterceptor } from '@utilities/check-token-interceptor';
import { AuthConstants } from '@constants/auth.constant';

export function tokenGetter() {
    return localStorage.getItem(AuthConstants.TOKEN);
}

@NgModule({
    declarations: [
        AppComponent,
        NotfoundComponent
    ],
    imports: [
        AppRoutingModule,
        AppLayoutModule,
        HttpClientModule,
        ToastModule,
        ConfirmDialogModule,
        NgxProgressHttpModule,
        ScrollTopModule,
        JwtModule.forRoot({
            config: {
                tokenGetter: tokenGetter,
                allowedDomains: environment.allowedDomains,
                disallowedRoutes: environment.disallowedRoutes,
            },
        }),
        QuillModule.forRoot(),
    ],
    providers: [
        { provide: LocationStrategy, useClass: PathLocationStrategy },
        MessageService,
        ConfirmationService,
        {
            provide: APP_INITIALIZER,
            useFactory: RefreshTokenInitializer,
            multi: true,
            deps: [AuthAdminService]
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: CheckTokenInterceptor,
            multi: true
        }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
