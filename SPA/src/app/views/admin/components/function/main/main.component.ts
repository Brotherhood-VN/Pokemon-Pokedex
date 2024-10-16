import { Component, OnInit } from '@angular/core';
import { FormGroupComponent } from '../form-group/form-group.component';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ActionDetail, FunctionSearchParam, FunctionView } from '@models/admins/function';
import { InjectBase } from '@utilities/inject-base';
import { MenuItem } from 'primeng/api';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { FunctionService } from '@services/admin/function.service';
import { Function } from '@models/admins/function'
import { OperationResult } from '@utilities/operation-result';
import { FormComponent } from '../form/form.component';
import { CaptionConstants, MessageConstants } from '@constants/message.constant';

interface expandedRows {
  [key: string]: boolean;
}

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent extends InjectBase implements OnInit {
  // Breadcrumb
  home: MenuItem = <MenuItem>{ icon: 'pi pi-home', routerLink: '/admin' };
  breadcrumbs: MenuItem[] = [{ label: 'Function' }];

  functions: FunctionView[] = [];
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0
  }

  params: FunctionSearchParam = <FunctionSearchParam>{};

  rowsPerPageOptions: number[] = [5, 10, 25, 50];

  contextItems!: MenuItem[];
  selectedFunction!: FunctionView;

  ref: DynamicDialogRef | undefined;

  expandedRows: expandedRows = {};
  isExpanded: boolean = false;

  constructor(
    private _functionService: FunctionService, 
    private _dialogService: DialogService) {
    super();
  }

  ngOnInit(): void {
    // this.contextItems = [
    //   { label: 'Chỉnh sửa', icon: 'pi pi-pencil', command: () => this.updateGroup(this.selectedFunction) },
    //   { label: 'Xóa', icon: 'pi pi-trash', command: () => this.deleteGroup() },
    //   { separator: true },
    //   { label: 'Bỏ chọn', icon: 'pi pi-times', command: () => this.closeMenuContext() }
    // ];

    this.clearSearch();
  }

  //#region init and change data
  getDataPagination() {
    this._progress.start();

    this._functionService.getDataPagination(this.pagination, this.params)
      .subscribe({
        next: (res: PaginationResult<FunctionView>) => {
          this.functions = res.result;
          this.pagination = res.pagination;
        }
      }).add(() => this._progress.end());
  }

  search() {
    this.pagination.pageNumber = 1;
    this.getDataPagination();
  }

  clearSearch() {
    this.params = <FunctionSearchParam>{};
    this.search();
  }

  onPageChange(event: any) {
    this.pagination.pageNumber = event.page + 1;
    this.pagination.pageSize = event.rows;
    this.getDataPagination();
  }
  //#endregion

  //#region create vs create group data
  create(func: FunctionView) {
    let funct: Function = <Function>{
      area: func.area,
      controller: func.controller,
      title: func.title,
      isDelete: false,
      isMenu: false,
      isShow: true,
      seq: 1,
      isUpdate: false
    }

    this.showForm(funct);
  }

  createGroup() {
    let funct: FunctionView = <FunctionView>{
      actionDetails: [],
      isUpdate: false
    }

    this.showFormGroup(funct);
  }
  //#endregion

  //#region update vs update group data
  update(functionGroup: FunctionView, act: ActionDetail) {
    let func: Function = <Function>{
      area: functionGroup.area,
      controller: functionGroup.controller,
      title: functionGroup.title,
      id: act.id,
      action: act.action,
      description: act.description,
      isMenu: act.isMenu,
      isShow: act.isShow,
      seq: act.seq,
      isUpdate: true
    }
    console.log(functionGroup)
    this.showForm(func);
  }

  updateGroup(func: FunctionView) {
    func.isUpdate = true;
    this.showFormGroup(func);
  }
  //#endregion

  //#region delete vs delete group data
  delete(functionGroup: FunctionView, act: ActionDetail) {
    let func: Function = <Function>{
      id: act.id,
      action: act.action,
      title: functionGroup.title,
      description: act.description,
      isMenu: act.isMenu,
      isShow: act.isShow,
      seq: act.seq
    }
    this._toast.confirm('Are you sure?', MessageConstants.CONFIRM_DELETE_MSG, () => this.acceptDelete('delete', func));
  }

  deleteGroup() {
    this._toast.confirm('Are you sure?', MessageConstants.CONFIRM_DELETE_MSG, () => this.acceptDelete('deleteRange', this.selectedFunction));
  }

  acceptDelete(method: string, data: Function | FunctionView) {
    this._progress.start();

    this._functionService[method](data)
      .subscribe({
        next: (res: OperationResult) => {
          if (res.isSuccess) {
            this._toast.success('Delete', 'Xóa chức năng thành công !');
            this.search();
          }
          else
            this._toast.error(CaptionConstants.ERROR, res.message ?? 'Xóa chức năng có lỗi khi lưu !');
        },
        error: () => {
          this._toast.error(CaptionConstants.ERROR, MessageConstants.UN_KNOWN_ERROR);
        }
      }).add(() => this._progress.end());
  }
  //#endregion

  closeMenuContext() {
    this.selectedFunction = null;
  }

  //#region toggle expand all group data
  expandAll() {
    if (!this.isExpanded) {
      this.functions.forEach(func => func && func.controller ? this.expandedRows[func.controller] = true : '');

    } else {
      this.expandedRows = {};
    }
    this.isExpanded = !this.isExpanded;
  }
  //#endregion

  //#region showForm - showFormGroup => create vs update functions
  showForm(func: Function) {
    let config: DynamicDialogConfig<Function> = <DynamicDialogConfig<Function>>{
      header: !func.isUpdate ? 'Thêm mới' : 'Chỉnh sửa',
      appendTo: 'body',
      data: func,
      width: '50vw',
      contentStyle: { overflow: 'auto' },
      breakpoints: {
        '1199px': '75vw',
        '575px': '90vw'
      },
      modal: true,
      maximizable: true
    };
    this.ref = this._dialogService.open(FormComponent, config);

    this.ref.onClose.subscribe({
      next: (isReloadData: boolean) => {
        this.selectedFunction = null;
        this.ref.destroy();

        if (isReloadData)
          this.search();
      }
    });
  }

  showFormGroup(functionGroup: FunctionView) {
    const clone: string = JSON.stringify(functionGroup);

    let config: DynamicDialogConfig<FunctionView> = <DynamicDialogConfig<FunctionView>>{
      header: !functionGroup.isUpdate ? 'Thêm mới' : 'Chỉnh sửa',
      appendTo: 'body',
      data: functionGroup,
      width: '70vw',
      contentStyle: { overflow: 'auto' },
      breakpoints: {
        '1199px': '75vw',
        '575px': '90vw'
      },
      modal: true,
      maximizable: true
    };
    this.ref = this._dialogService.open(FormGroupComponent, config);

    this.ref.onClose.subscribe({
      next: (isReloadData: boolean) => {
        this.selectedFunction = null;
        this.ref.destroy();

        if (isReloadData)
          this.search();
        else if (functionGroup.isUpdate == true)
          this.functions.splice(this.functions.findIndex(x => x.controller == functionGroup.controller), 1, JSON.parse(clone))
      }
    });
  }
  //#endregion
}
