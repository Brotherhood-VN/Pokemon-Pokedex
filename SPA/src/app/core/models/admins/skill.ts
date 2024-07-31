import { BaseModel } from "./base-model";

export interface Skill extends BaseModel {
    level: number | null;
    itemId: number | null;
}