export interface Account {
    id: string;
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
    id: string;
    password: string;
    newPassword: string;
    confirmPassword: string;
    updateBy: string;
    updateTime: string | null | Date;
}