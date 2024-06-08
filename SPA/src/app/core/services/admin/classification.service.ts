import { Injectable } from '@angular/core';
import { BaseModel } from '@models/admins/base-model';
import { BaseService } from './_base.service';
import { KeyValuePair } from '@utilities/key-value-utility';

@Injectable({
  providedIn: 'root'
})
export class ClassificationService extends BaseService<BaseModel> {
  
  constructor() {
    super("Classification");
  }

  getListClassification() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/${this.controller}/GetListClassification`, {});
  }
}
