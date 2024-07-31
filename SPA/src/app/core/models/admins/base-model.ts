export interface BaseModel {
    id: number;
    code: string;
    title: string;
    description: string;
    status: boolean;
    isDelete: boolean;
    createBy: number;
    createTime: string | Date;
    updateBy: number | number;
    updateTime: string | null | Date;
}