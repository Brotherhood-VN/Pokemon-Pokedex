import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Menu } from '@models/admins/menu';
import { KeyValuePair } from '@utilities/key-value-utility';
import { OperationResult } from '@utilities/operation-result';
import { TreeNode } from 'primeng/api';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  apiUrl: string = environment.apiUrl;
  constructor(private _http: HttpClient) { }

  loadMenus(): Observable<TreeNode<Menu>[]> {
    return this._http.get<TreeNode<Menu>[]>(`${this.apiUrl}/Menu/LoadMenus`, {});
  }

  configurationMenus(nodes: TreeNode<Menu>[]): Observable<OperationResult> {
    return this._http.post<OperationResult>(`${this.apiUrl}/Menu/ConfigurationMenus`, nodes);
  }

  create(menu: Menu): Observable<OperationResult> {
    return this._http.post<OperationResult>(`${this.apiUrl}/Menu/Create`, menu);
  }

  update(menu: Menu): Observable<OperationResult> {
    return this._http.put<OperationResult>(`${this.apiUrl}/Menu/Update`, menu);
  }

  delete(id: string): Observable<OperationResult> {
    return this._http.delete<OperationResult>(`${this.apiUrl}/Menu/Delete`, { params: { 'id': id } });
  }

  getListController(): Observable<KeyValuePair[]> {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/Menu/GetListController`, {});
  }
}
