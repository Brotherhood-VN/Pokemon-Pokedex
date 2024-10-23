import { Injectable } from '@angular/core';
import { FunctionConstants } from '@constants/function.constant';
import { KeyValuePair } from '@utilities/key-value-utility';
import { Observable } from 'rxjs';
import { BaseDoubleService, BaseService, IBaseDoubleService, IBaseService } from './_base.service';

export interface IBaseSystemService<T> extends IBaseService<T> {
  getListEntities(): Observable<KeyValuePair[]>;
}

@Injectable({
  providedIn: 'root'
})
export class BaseSystemService<T> extends BaseService<T> implements IBaseSystemService<T> {
  getListEntities(id?: string): Observable<KeyValuePair[]> {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/${this.controller}/${FunctionConstants.GETLISTENTITIES}`, { params: { id } });
  }
}

/** ---------------------------------------------- Double ---------------------------------------------- */

export interface IBaseSystemDoubleService<T, M> extends IBaseDoubleService<T, M> {
  getListEntities(): Observable<KeyValuePair[]>;
  getListDetails(): Observable<M[]>;
}

@Injectable({
  providedIn: 'root'
})
export class BaseSystemDoubleService<T, M> extends BaseDoubleService<T, M> implements IBaseSystemDoubleService<T, M> {
  getListEntities(id?: string): Observable<KeyValuePair[]> {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/${this.controller}/${FunctionConstants.GETLISTENTITIES}`, { params: { id } });
  }

  getListDetails(id?: string): Observable<M[]> {
    return this._http.get<M[]>(`${this.apiUrl}/${this.controller}/${FunctionConstants.GETLISTENTITIES}`, { params: { id } });
  }
}