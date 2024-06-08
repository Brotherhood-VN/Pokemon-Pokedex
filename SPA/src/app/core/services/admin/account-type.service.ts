import { Injectable } from '@angular/core';
import { BaseService } from './_base.service';
import { KeyValuePair } from '@utilities/key-value-utility';
import { BaseModel } from '@models/admins/base-model';

@Injectable({
  providedIn: 'root'
})
export class AccountTypeService extends BaseService<BaseModel> {
  
  constructor() {
    super("AccountType");
  }

  getListAccountType() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/${this.controller}/GetListAccountType`, {});
  }
}
