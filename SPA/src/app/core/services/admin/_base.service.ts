import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { Observable } from 'rxjs';

export interface IBaseService<T> {
  create(model: T): Observable<OperationResult>;

  update(model: T): Observable<OperationResult>;

  delete(model: T): Observable<OperationResult>;

  getDataPagination(params?: any): Observable<PaginationResult<T>>;

  getDetail(id: string): Observable<T>;
}

@Injectable({
  providedIn: 'root'
})
export class BaseService<T> implements IBaseService<T> {
  apiUrl: string = environment.apiUrl;
  protected _http: HttpClient = inject(HttpClient);

  constructor(
    @Inject(String) protected controller: string
  ) { }

  create(model: T): Observable<OperationResult> {
    return this._http.post<OperationResult>(`${this.apiUrl}/${this.controller}/Create`, model);
  }

  update(model: T): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/${this.controller}/Update`, model);
  }

  delete(model: T): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/${this.controller}/Delete`, model);
  }

  getDataPagination(pagination: Pagination, params?: any): Observable<PaginationResult<T>> {
    let queries = new HttpParams().appendAll({ ...pagination });
    if (params && typeof (params) != 'string')
      queries = queries.appendAll({ ...params });
    else
      queries = queries.append('Keyword', params);

    return this._http.get<PaginationResult<T>>(`${this.apiUrl}/${this.controller}/GetDataPagination`, { params: queries });
  }

  getDetail(id: string): Observable<T> {
    return this._http.get<T>(`${this.apiUrl}/${this.controller}/GetDetail`, { params: { 'id': id } });
  }
}
