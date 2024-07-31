import { Injectable } from '@angular/core';
import { BaseService } from './_base.service';
import { KeyValuePair } from '@utilities/key-value-utility';
import { Classification } from '@models/admins/classification';

@Injectable({
  providedIn: 'root'
})
export class ClassificationService extends BaseService<Classification> {
  
  constructor() {
    super("Classification");
  }

  getListClassification() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/${this.controller}/GetListClassification`, {});
  }
}
