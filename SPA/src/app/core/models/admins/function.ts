import { KeyValuePair } from "@utilities/key-value-utility";

export interface FunctionView {
    area: string;
    controller: string;
    title: string;
    actionDetails: ActionDetail[];
    createBy: number;
    createTime: string | Date;
    updateBy: number | null;
    updateTime: string | null | Date;
    isUpdate?: boolean;
}

export interface ActionDetail {
    id: number;
    action: string;
    description: string;
    isShow: boolean;
    isMenu: boolean | null;
    seq: number | null;
    isDelete: boolean | null;
    selection?: KeyValuePair;
    isNew?: boolean;
}

export interface Function {
    id: number;
    area: string;
    controller: string;
    action: string;
    title: string;
    description: string;
    isShow: boolean;
    isMenu: boolean | null;
    seq: number | null;
    isDelete: boolean | null;
    createBy: number;
    createTime: string | Date;
    updateBy: number | null;
    updateTime: string | null | Date;
    isUpdate?: boolean;
}

export interface FunctionParam {
    title: string;
    description: string;
    area: string;
    controller: string;
    action: string;
}

export interface FunctionSearchParam {
    area: string;
    controller: string;
    keyword: string;
}