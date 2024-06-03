import { AuthAdminService } from "@services/auth/auth-admin.service";

export function RefreshTokenInitializer(authService: AuthAdminService) {
    return () => new Promise((resolve: any) => {
        authService.refreshToken().subscribe().add(resolve)
    });
}