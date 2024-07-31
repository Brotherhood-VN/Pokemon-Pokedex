import { Injectable } from '@angular/core';
import { BaseService } from './_base.service';
import { KeyValuePair } from '@utilities/key-value-utility';
import { Region } from '@models/admins/region';

@Injectable({
  providedIn: 'root'
})
export class RegionService extends BaseService<Region> {
  
  constructor() {
    super("Region");
  }

  getListRegion() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/${this.controller}/GetListRegion`, {});
  }
}
