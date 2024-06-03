import { Menu } from '@models/admins/menu';

export interface Auth {
    accountId: string;
    avartar: string;
    type: string;
    fullName: string;
    position: string;
    branchId: string | null;
}

export interface UserForLoginParam {
    username: string;
    password: string;
    branchId: string | null;
    rememberMe: boolean;
}

export interface TokenRequestParam {
    token: string;
    branchId: string | null;
}

export interface UserReturned {
    token: string;
    refreshToken: string;
    area: string;
    user: Auth;
    menus: Menu[];
}

export interface ResetPassword {
    email: string;
    code?: string;
    password: string;
}