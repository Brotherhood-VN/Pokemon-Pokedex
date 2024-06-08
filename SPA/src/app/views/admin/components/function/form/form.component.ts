import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { FunctionService } from '@services/admin/function.service';
import { InjectBase } from '@utilities/inject-base';
import { Function } from '@models/admins/function';
import { OperationResult } from '@utilities/operation-result';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { CaptionConstants, MessageConstants } from '@constants/message.constant';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrl: './form.component.scss'
})
export class FormComponent extends InjectBase implements OnInit {
  func: Function = <Function>{};

  constructor(
    private _ref: DynamicDialogRef,
    private _config: DynamicDialogConfig,
    private _functionService: FunctionService) {
    super();
  }

  ngOnInit(): void {
    this.func = this._config.data;
  }

  save(functionForm: NgForm) {
    let method = !this.func.isUpdate ? 'create' : 'update';
    let success = !this.func.isUpdate ? 'Thêm mới chức năng thành công !' : 'Chỉnh sửa chức năng thành công !';
    let error = !this.func.isUpdate ? 'Thêm mới chức năng có lỗi khi lưu !' : 'Chỉnh sửa chức năng có lỗi khi lưu !';

    this.execute(functionForm, method, success, error);
  }

  execute(functionForm: NgForm, method: string, success: string, error: string) {
    this._progress.start();
    this._functionService[method](this.func)
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
    functionForm.reset();
    this._ref.close(isReloadData);
  }
}
