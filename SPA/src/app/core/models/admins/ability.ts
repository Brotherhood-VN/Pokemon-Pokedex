import { BaseModel } from './base-model';

export interface Ability extends BaseModel {
    effect: string;
    inDepthEffect: string;
}
