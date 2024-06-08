import { Injectable } from '@angular/core';
import { BaseService } from './_base.service';
import { Role, RoleUser, RoleUserParams } from '@models/admins/role';
import { KeyValuePair } from '@utilities/key-value-utility';

@Injectable({
  providedIn: 'root'
})
export class RoleService extends BaseService<Role> {

  constructor() {
    super("Role");
  }

  getListRole() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/${this.controller}/GetListRole`, {});
  }

  getRoles(params: RoleUserParams) {
    return this._http.get<RoleUser[]>(`${this.apiUrl}/${this.controller}/GetRoles`, { params: { ...params } });
  }
}