import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable, inject } from '@angular/core';
import { FunctionConstants } from '@constants/function.constant';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { Observable } from 'rxjs';

export interface IBaseService<T> {
  create(model: T): Observable<OperationResult>;

  update(model: T): Observable<OperationResult>;

  delete(model: T): Observable<OperationResult>;

  getDataPagination(params?: any): Observable<PaginationResult<T>>;

  getDetail(id: number): Observable<T>;
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
    return this._http.post<OperationResult>(`${this.apiUrl}/${this.controller}/${FunctionConstants.CREATE}`, model);
  }

  update(model: T): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/${this.controller}/${FunctionConstants.UPDATE}`, model);
  }

  delete(model: T): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/${this.controller}/${FunctionConstants.DELETE}`, model);
  }

  getDataPagination(pagination: Pagination, params?: any): Observable<PaginationResult<T>> {
    let queries = new HttpParams().appendAll({ ...pagination });
    if (params && typeof (params) != 'string')
      queries = queries.appendAll({ ...params });
    else
      queries = queries.append('Keyword', params);

    return this._http.get<PaginationResult<T>>(`${this.apiUrl}/${this.controller}/${FunctionConstants.GETDATAPAGINATION}`, { params: queries });
  }

  getDetail(id: number): Observable<T> {
    return this._http.get<T>(`${this.apiUrl}/${this.controller}/${FunctionConstants.GETDETAIL}`, { params: { 'id': id } });
  }
}


/** ---------------------------------------------- Double ---------------------------------------------- */
export interface IBaseDoubleService<T, M> {
  create(model: T): Observable<OperationResult>;

  update(model: T): Observable<OperationResult>;

  delete(model: M): Observable<OperationResult>;

  getDataPagination(params?: any): Observable<PaginationResult<M>>;

  getDetail(id: number): Observable<T>;
}

@Injectable({
  providedIn: 'root'
})
export class BaseDoubleService<T, M> implements IBaseDoubleService<T, M> {
  apiUrl: string = environment.apiUrl;
  protected _http: HttpClient = inject(HttpClient);

  constructor(
    @Inject(String) protected controller: string
  ) { }

  create(model: T): Observable<OperationResult> {
    return this._http.post<OperationResult>(`${this.apiUrl}/${this.controller}/${FunctionConstants.CREATE}`, model);
  }

  update(model: T): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/${this.controller}/${FunctionConstants.UPDATE}`, model);
  }

  delete(model: M): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/${this.controller}/${FunctionConstants.DELETE}`, model);
  }

  getDataPagination(pagination: Pagination, params?: any): Observable<PaginationResult<M>> {
    let queries = new HttpParams().appendAll({ ...pagination });
    if (params && typeof (params) != 'string')
      queries = queries.appendAll({ ...params });
    else
      queries = queries.append('Keyword', params);

    return this._http.get<PaginationResult<M>>(`${this.apiUrl}/${this.controller}/${FunctionConstants.GETDATAPAGINATION}`, { params: queries });
  }

  getDetail(id: number): Observable<T> {
    return this._http.get<T>(`${this.apiUrl}/${this.controller}/${FunctionConstants.GETDETAIL}`, { params: { 'id': id } });
  }
}


/** ---------------------------------------------- Triple ---------------------------------------------- */
export interface IBaseTripleService<T, M, D> {
  create(model: T): Observable<OperationResult>;

  update(model: T): Observable<OperationResult>;

  delete(model: M): Observable<OperationResult>;

  getDataPagination(params?: any): Observable<PaginationResult<M>>;

  getDetail(id: number): Observable<D>;
}

@Injectable({
  providedIn: 'root'
})
export class BaseTripleService<T, M, D> implements IBaseTripleService<T, M, D> {
  apiUrl: string = environment.apiUrl;
  protected _http: HttpClient = inject(HttpClient);

  constructor(
    @Inject(String) protected controller: string
  ) { }

  create(model: T): Observable<OperationResult> {
    return this._http.post<OperationResult>(`${this.apiUrl}/${this.controller}/${FunctionConstants.CREATE}`, model);
  }

  update(model: T): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/${this.controller}/${FunctionConstants.UPDATE}`, model);
  }

  delete(model: M): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/${this.controller}/${FunctionConstants.DELETE}`, model);
  }

  getDataPagination(pagination: Pagination, params?: any): Observable<PaginationResult<M>> {
    let queries = new HttpParams().appendAll({ ...pagination });
    if (params && typeof (params) != 'string')
      queries = queries.appendAll({ ...params });
    else
      queries = queries.append('Keyword', params);

    return this._http.get<PaginationResult<M>>(`${this.apiUrl}/${this.controller}/${FunctionConstants.GETDATAPAGINATION}`, { params: queries });
  }

  getDetail(id: number): Observable<D> {
    return this._http.get<D>(`${this.apiUrl}/${this.controller}/${FunctionConstants.GETDETAIL}`, { params: { 'id': id } });
  }
}
