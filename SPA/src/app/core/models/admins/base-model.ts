export interface BaseModel {
    id: string;
    code: string;
    title: string;
    description: string;
    status: boolean;
    isDelete: boolean;
    createBy: string;
    createTime: string | Date;
    updateBy: string;
    updateTime: string | null | Date;
}