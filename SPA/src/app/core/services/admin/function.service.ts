import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { FunctionParam, FunctionSearchParam, FunctionView } from '@models/admins/function';
import { KeyValuePair } from '@utilities/key-value-utility';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FunctionService {
  apiUrl: string = environment.apiUrl;
  constructor(private _http: HttpClient) { }

  create(func: Function): Observable<OperationResult> {
    return this._http.post<OperationResult>(`${this.apiUrl}/Function/Create`, func);
  }

  createRange(func: FunctionView): Observable<OperationResult> {
    return this._http.post<OperationResult>(`${this.apiUrl}/Function/CreateRange`, func);
  }

  update(func: Function): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/Function/Update`, func);
  }

  updateRange(func: FunctionView): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/Function/UpdateRange`, func);
  }

  delete(func: Function): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/Function/Delete`, func);
  }

  deleteRange(func: FunctionView): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/Function/DeleteRange`, func);
  }

  getDataPagination(pagination: Pagination, params: FunctionSearchParam): Observable<PaginationResult<FunctionView>> {
    return this._http.get<PaginationResult<FunctionView>>(`${this.apiUrl}/Function/GetDataPagination`, { params: { ...pagination, ...params } });
  }

  getDetail(params: FunctionParam): Observable<PaginationResult<Function>> {
    return this._http.get<PaginationResult<Function>>(`${this.apiUrl}/Function/GetDetail`, { params: { ...params } });
  }

  getDetailGroup(params: FunctionParam): Observable<PaginationResult<FunctionView>> {
    return this._http.get<PaginationResult<FunctionView>>(`${this.apiUrl}/Function/GetDetailGroup`, { params: { ...params } });
  }

  getListArea(): Observable<KeyValuePair[]> {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/Function/GetListArea`, {});
  }

  getListController(area: string): Observable<KeyValuePair[]> {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/Function/GetListController`, { params: { area } });
  }

  getListAction(area: string, controller: string): Observable<KeyValuePair[]> {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/Function/GetListAction`, { params: { area, controller } });
  }
}
