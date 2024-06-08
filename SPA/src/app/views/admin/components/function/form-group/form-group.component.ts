import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CaptionConstants, MessageConstants } from '@constants/message.constant';
import { ActionDetail, FunctionView } from '@models/admins/function';
import { FunctionService } from '@services/admin/function.service';
import { InjectBase } from '@utilities/inject-base';
import { KeyValuePair } from '@utilities/key-value-utility';
import { OperationResult } from '@utilities/operation-result';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-form-group',
  templateUrl: './form-group.component.html',
  styleUrl: './form-group.component.scss'
})
export class FormGroupComponent extends InjectBase implements OnInit {
  areas: KeyValuePair[] = [];
  area: KeyValuePair = null;

  controllers: KeyValuePair[] = [];
  controller: KeyValuePair = null;

  actions: KeyValuePair[] = [];

  funcGroup: FunctionView = <FunctionView>{};
  entityClone: ActionDetail = <ActionDetail>{};

  addMode: boolean = false;
  disabled: boolean = false;

  constructor(
    private _ref: DynamicDialogRef,
    private _config: DynamicDialogConfig,
    private _functionService: FunctionService) {
    super();
  }

  ngOnInit(): void {
    this.funcGroup = this._config.data;

    this.getListArea();
  }

  getListArea() {
    this._progress.start();
    this._functionService.getListArea()
      .subscribe({
        next: (res) => {
          this.areas = res;

          if (this.funcGroup.area)
            this.area = this.areas.find(x => x.key == this.funcGroup.area);
          else
            this.area = this.areas[0];

          if (this.area)
            this.getListController(this.area.key);
        }
      }).add(() => this._progress.end());
  }

  onAreaChange(event: KeyValuePair) {
    this.funcGroup.area = '';
    this.funcGroup.controller = '';
    if (event) {
      this.funcGroup.area = event.key;
      this.getListController(this.funcGroup.area);
    }
  }

  getListController(area: string) {
    this._progress.start();
    this._functionService.getListController(area)
      .subscribe({
        next: (res) => {
          this.controllers = res;

          if (this.funcGroup.controller)
            this.controller = this.controllers.find(x => x.key == this.funcGroup.controller);

          if (this.controller)
            this.getListAction(this.funcGroup.area, this.funcGroup.controller);
        }
      }).add(() => this._progress.end());
  }

  onControllerChange(event: KeyValuePair) {
    this.funcGroup.controller = '';
    this.funcGroup.title = '';
    if (event) {
      this.funcGroup.controller = event.key;
      this.funcGroup.title = event.value;

      this.getListAction(this.funcGroup.area, this.funcGroup.controller);
    }
  }

  getListAction(area: string, controller: string) {
    this._progress.start();
    this._functionService.getListAction(area, controller)
      .subscribe({
        next: (res) => {
          this.actions = res;

          this.funcGroup.actionDetails.forEach(x => {
            x.selection = res.find(y => y.key == x.action);
          })
        }
      }).add(() => this._progress.end());
  }

  onActionChange(event: KeyValuePair, act: ActionDetail) {
    act.action = '';
    act.description = '';
    if (event) {
      act.action = event.key;
      act.description = event.value;
    }
  }

  addRow(table: Table) {
    this._config.closable = false;
    this._config.closeOnEscape = false;
    this.addMode = true;
    this.disabled = true;
    let item = <ActionDetail>{
      // id: FunctionUtility.generateGUID(),
      isDelete: false,
      isShow: true,
      isMenu: false,
      seq: this.funcGroup.actionDetails.length > 0 ? Math.max(...this.funcGroup.actionDetails.map(x => x.seq)) + 1 : 1,
      selection: null,
      isNew: true
    }

    this.funcGroup.actionDetails.unshift(item);
    console.log(this.funcGroup.actionDetails);
    
    table.reset();
    table.initRowEdit(item);
  }

  editRow(table: Table, item: ActionDetail) {
    this._config.closable = false;
    this._config.closeOnEscape = false;
    this.disabled = true;
    this.entityClone = { ...item };
    table.initRowEdit(item);
  }

  deleteRow(item: ActionDetail) {
    this._toast.confirm('Are you sure?', MessageConstants.CONFIRM_DELETE_MSG, () => this.funcGroup.actionDetails.remove(item));
  }

  saveRow(table: Table, item: ActionDetail) {
    this._config.closable = true;
    this._config.closeOnEscape = true;
    this.disabled = false;
    if (this.addMode) {
      this.addMode = false;
    }
    else {
      this.entityClone = <ActionDetail>{};
    }
    table.cancelRowEdit(item);
  }

  cancel(table: Table, item: ActionDetail) {
    this._config.closable = true;
    this._config.closeOnEscape = true;
    this.disabled = false;
    if (this.addMode) {
      this.funcGroup.actionDetails.shift();
      this.addMode = false;
    }
    else {
      this.funcGroup.actionDetails.splice(this.funcGroup.actionDetails.indexOf(item), 1, this.entityClone);
      this.entityClone = <ActionDetail>{};
    }
    table.cancelRowEdit(item);
  }

  save(functionForm: NgForm) {
    let method = !this.funcGroup.isUpdate ? 'createRange' : 'updateRange';
    let success = !this.funcGroup.isUpdate ? 'Thêm mới chức năng thành công !' : 'Chỉnh sửa chức năng thành công !';
    let error = !this.funcGroup.isUpdate ? 'Thêm mới chức năng có lỗi khi lưu !' : 'Chỉnh sửa chức năng có lỗi khi lưu !';

    this.execute(functionForm, method, success, error);
  }

  execute(functionForm: NgForm, method: string, success: string, error: string) {
    this._progress.start();
    this._functionService[method](this.funcGroup)
      .subscribe({
        next: (res: OperationResult) => {
          if (res.isSuccess) {
            this._toast.success(CaptionConstants.SUCCESS, success);
            this.close(functionForm, true);
          }
          else
            this._toast.error(CaptionConstants.ERROR, res.message ?? error);
        },
        error: () => {
          this._toast.error(CaptionConstants.ERROR, MessageConstants.UN_KNOWN_ERROR);
        }
      }
      ).add(() => this._progress.end());
  }

  close(functionForm: NgForm, isReloadData: boolean = false) {
    if (!this.funcGroup.isUpdate)
      functionForm.reset();

    this._ref.close(isReloadData);
  }
}
