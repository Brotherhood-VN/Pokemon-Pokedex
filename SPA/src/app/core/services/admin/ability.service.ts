import { Injectable } from '@angular/core';
import { BaseService } from './_base.service';
import { KeyValuePair } from '@utilities/key-value-utility';
import { Ability } from '@models/admins/ability';

@Injectable({
  providedIn: 'root'
})
export class AbilityService extends BaseService<Ability> {
  
  constructor() {
    super("Ability");
  }

  getListAbility() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/${this.controller}/GetListAbility`, {});
  }
}
