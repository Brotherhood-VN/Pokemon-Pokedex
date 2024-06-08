import { Injectable } from '@angular/core';
import { BaseService } from './_base.service';
import { KeyValuePair } from '@utilities/key-value-utility';
import { Skill } from '@models/admins/skill';

@Injectable({
  providedIn: 'root'
})
export class SkillService extends BaseService<Skill> {
  
  constructor() {
    super("Skill");
  }

  getListSkill() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/${this.controller}/GetListSkill`, {});
  }
}