export interface Role {
    id: string;
    code: string;
    title: string;
    status: boolean | null;
    isDelete: boolean | null;
    createBy: string;
    createTime: string | Date;
    updateBy: string | null;
    updateTime: string | null | Date;
    functionIds: string[];
}

export interface RoleAuth {
    id: string;
    area: string;
    controller: string;
    action: string;
    title: string;
    parentId: string | null;
    isShow: boolean | null;
    isActive: boolean;
}

export interface RoleUser {
    id: string;
    controller: string;
    title: string;
    isChecked: boolean;
    isDisabled: boolean;
    actions: SubRoleUser[];
}

export interface SubRoleUser {
    id: string;
    area: string;
    controller: string;
    action: string;
    title: string;
    parentId: string;
    seq: number;
    isChecked: boolean;
    isDisabled: boolean;
}

export interface RoleUserParams {
    checkedAll: boolean;
    roleIds: string[];
    accountId: string | null;
}