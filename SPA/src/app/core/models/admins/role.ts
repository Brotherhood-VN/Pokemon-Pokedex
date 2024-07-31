export interface Role {
    id: number;
    code: string;
    title: string;
    description: string;
    status: boolean | null;
    isDelete: boolean | null;
    createBy: number;
    createTime: string | Date;
    updateBy: number | null;
    updateTime: string | null | Date;
    functionIds: number[];
}

export interface RoleAuth {
    id: number;
    area: string;
    controller: string;
    action: string;
    title: string;
    parentId: number | null;
    isShow: boolean | null;
    isActive: boolean;
}

export interface RoleUser {
    id: number;
    controller: string;
    title: string;
    isChecked: boolean;
    isDisabled: boolean;
    actions: SubRoleUser[];
}

export interface SubRoleUser {
    id: number;
    area: string;
    controller: string;
    action: string;
    title: string;
    parentId: number;
    seq: number;
    isChecked: boolean;
    isDisabled: boolean;
}

export interface RoleUserParams {
    checkedAll: boolean;
    roleIds: number[];
    accountId: number | null;
}