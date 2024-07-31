import { Menu } from '@models/admins/menu';

export interface Auth {
    accountId: number;
    avartar: string;
    type: string;
    fullName: string;
}

export interface UserForLoginParam {
    username: string;
    password: string;
    rememberMe: boolean;
}

export interface TokenRequestParam {
    token: string;
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