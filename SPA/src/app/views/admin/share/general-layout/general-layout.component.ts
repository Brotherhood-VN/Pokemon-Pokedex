import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CaptionConstants, MessageConstants } from '@constants/message.constant';
import { BaseModel } from '@models/admins/base-model';
import { BaseService } from '@services/admin/_base.service';
import { InjectBase } from '@utilities/inject-base';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { MenuItem } from 'primeng/api';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-general-layout',
  templateUrl: './general-layout.component.html',
  styleUrl: './general-layout.component.scss'
})
export class GeneralLayoutComponent extends InjectBase {
  // Breadcrumb
  home: MenuItem = <MenuItem>{ icon: 'pi pi-home', routerLink: '/admin' };
  breadcrumbs: MenuItem[] = [];

  title: string = '';
  controller: string = '';
  _service: BaseService<BaseModel>;

  data: BaseModel[] = [];

  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0
  }

  rowsPerPageOptions: number[] = [5, 10, 25, 50];

  addMode: boolean = false;
  disabled: boolean = false;

  keyword: string = '';

  entityClone: BaseModel = <BaseModel>{};

  keyCode: string = '';

  constructor(private _route: ActivatedRoute) {
    super();

    const routerData = this._route.snapshot.data;
    this.title = routerData['title'];
    this.controller = routerData['controller'];
    this._service = new BaseService<BaseModel>(this.controller);

    this.breadcrumbs.push({ label: this.controller });
    this.keyCode = this.controller.split('').filter(x => /[A-Z]/.test(x)).join('');
  }

  ngOnInit() {
    this.search();
  }

  loadData() {
    this._progress.start();
    this._service.getDataPagination(this.pagination, this.keyword)
      .subscribe({
        next: (res: PaginationResult<BaseModel>) => {
          this.data = res.result;
          this.pagination = res.pagination;
        }
      }).add(() => this._progress.end());
  }

  search() {
    this.pagination.pageNumber = 1;
    this.loadData();
  }

  clearSearch() {
    this.keyword = '';
    this.search();
  }

  onPageChange(event: any) {
    this.pagination.pageNumber = event.page + 1;
    this.pagination.pageSize = event.rows;
    this.loadData();
  }

  addRow(table: Table) {
    let code: number = this.data.length < 1 ? 1 : Math.max(...this.data.map(x => +x.code.substring(this.keyCode.length))) + 1;
    this.addMode = true;
    this.disabled = true;
    let item = <BaseModel>{
      code: `${this.keyCode}${code.toString().padStart(4, '0')}`,
      status: true,
      updateTime: new Date()
    }

    this.data.unshift(item);
    table.reset();
    table.initRowEdit(item);
  }

  editRow(table: Table, item: BaseModel) {
    this.disabled = true;
    this.entityClone = { ...item };
    item.updateTime = new Date(item.updateTime);
    table.initRowEdit(item);
  }

  cancel(table: Table, item: BaseModel) {
    this.disabled = false;
    if (this.addMode) {
      table.cancelRowEdit(item);
      this.data.shift();
      this.addMode = false;
    }
    else {
      this.data.splice(this.data.indexOf(item), 1, this.entityClone);
      this.entityClone = <BaseModel>{};
      table.cancelRowEdit(item);
    }
    this.disabled = false;
  }

  save(table: Table, item: BaseModel) {
    if (!item.title)
      return this._toast.warning('Warning', 'Tiêu đề không đc để trống !');

    this._progress.start();

    let method = this.addMode ? 'create' : 'update';
    let message = this.addMode ? 'Thêm mới' : 'Chỉnh sửa';

    this._service[method](item)
      .subscribe({
        next: (res: OperationResult) => {
          if (res.isSuccess) {
            this.disabled = false;
            this._toast.success(CaptionConstants.SUCCESS, `${message} ${this.title.toLowerCase()} thành công !`);
            if (this.addMode) {
              table.cancelRowEdit(item);
              this.addMode = false;
              this.search();
            }
            else {
              table.cancelRowEdit(this.entityClone);
              this.entityClone = <BaseModel>{};
            }
          }
          else
            this._toast.error(CaptionConstants.ERROR, res.message ?? `${message} ${this.title.toLowerCase()} có lỗi khi lưu !`);
        },
        error: () => {
          this._toast.error(CaptionConstants.ERROR, MessageConstants.UN_KNOWN_ERROR);
        }
      }).add(() => this._progress.end());
  }

  delete(item: BaseModel) {
    this._toast.confirm('Are you sure?', MessageConstants.CONFIRM_DELETE_MSG, () => this.acceptDelete(item));
  }

  acceptDelete(item: BaseModel) {
    this._progress.start();

    this._service.delete(item)
      .subscribe({
        next: (res: OperationResult) => {
          if (res.isSuccess) {
            this._toast.success(CaptionConstants.SUCCESS, `Xóa ${this.title.toLowerCase()} thành công !`);
            this.search();
          }
          else
            this._toast.error(CaptionConstants.ERROR, res.message ? res.message : `Xóa ${this.title.toLowerCase()} có lỗi khi lưu !`);
        },
        error: () => {
          this._toast.error(CaptionConstants.ERROR, MessageConstants.UN_KNOWN_ERROR);
        }
      }).add(() => this._progress.end());
  }
}
