export interface Account {
    id: number;
    userName: string;
    password: string;
    status: boolean | null;
    isDelete: boolean | null;
    createBy: string;
    createTime: string | Date;
    updateBy: string;
    updateTime: string | null | Date;
    accountTypeId: string;
    roleIds: string[];
    functionIds: string[];
}

export interface AccountChangePassword {
    id: number;
    password: string;
    newPassword: string;
    confirmPassword: string;
    updateBy: string;
    updateTime: string | null | Date;
}