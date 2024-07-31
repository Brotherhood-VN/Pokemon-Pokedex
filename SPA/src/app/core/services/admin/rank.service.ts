import { Injectable } from '@angular/core';
import { BaseService } from './_base.service';
import { BaseModel } from '@models/admins/base-model';
import { KeyValuePair } from '@utilities/key-value-utility';

@Injectable({
  providedIn: 'root'
})
export class RankService extends BaseService<BaseModel> {
  
  constructor() {
    super("Rank");
  }

  getListRank() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/${this.controller}/GetListRank`, {});
  }
}
