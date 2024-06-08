import { BaseModel } from "./base-model";

export interface Skill extends BaseModel {
    id: string;
    code: string;
    title: string;
    description: string;
    level: number | null;
    itemId: number | null;
    status: boolean;
    isDelete: boolean;
    createBy: string;
    createTime: string | Date;
    updateBy: string;
    updateTime: string | null | Date;
}