import { Component, OnInit } from '@angular/core';
import { CaptionConstants, MessageConstants } from '@constants/message.constant';
import { Role, RoleUser, RoleUserParams, SubRoleUser } from '@models/admins/role';
import { RoleService } from '@services/admin/role.service';
import { FunctionUtility } from '@utilities/function-utility';
import { InjectBase } from '@utilities/inject-base';
import { OperationResult } from '@utilities/operation-result';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrl: './form.component.scss'
})
export class FormComponent extends InjectBase implements OnInit {
  // Breadcrumb
  home: MenuItem = <MenuItem>{ icon: 'pi pi-home', routerLink: '/admin' };
  breadcrumbs: MenuItem[] = [{ label: 'Role', routerLink: '/admin/role' }, { label: 'Create' }];

  id: string = '';
  title: string = 'Thêm mới';
  role: Role = <Role>{
    code: FunctionUtility.generateASCII(8),
    status: true,
    functionIds: []
  };
  roles: RoleUser[] = [];
  maxAction: number = 5;
  params: RoleUserParams = <RoleUserParams>{
    checkedAll: true,
    roleIds: []
  }

  isCreate: boolean = true;
  checkAllRoles: boolean = false;
  // disableAllRoles: boolean = false;

  constructor(private _roleService: RoleService) {
    super();

    let extras = this._router.getCurrentNavigation()?.extras;
    this.id = extras?.state ? extras?.state['id'] : '';
  }

  ngOnInit() {
    if (this.id) {
      this.title = 'Chỉnh sửa';
      // this.params.roleIds.push(this.id);
      this.params.checkedAll = false;
      this.isCreate = false;

      this.getDetail();

      this.breadcrumbs.at(-1).label = 'Update';
    }
    this.getRoles();
  }

  getDetail() {
    this._progress.start();
    this._roleService.getDetail(this.id).subscribe({
      next: res => {
        this.role = res;
        if (!this.isCreate)
          sessionStorage.setItem('roles', JSON.stringify(this.roles));
      },
      error: () => this._toast.error(CaptionConstants.ERROR, MessageConstants.UN_KNOWN_ERROR),
      complete: () => this._progress.end()
    })
  }

  getRoles() {
    this._progress.start();

    this._roleService.getRoles(this.params)
      .subscribe({
        next: (res) => {
          this.maxAction = Math.max(...res.map(x => x.actions.length));
          res.filter(x => x.actions.length < this.maxAction).forEach(x => {
            let subs: SubRoleUser[] = [];
            for (let index = 0; index < this.maxAction - x.actions.length; index++) {
              subs.push({ id: x.actions[0].id } as SubRoleUser);
            }

            x.actions.push(...subs);
          });

          this.roles = res;
          this.checkAllRoles = this.roles.every(x => x.isChecked);
          // this.disableAllRoles = this.roles.every(x => x.isDisabled);
          if (!this.isCreate)
            sessionStorage.setItem('roles', JSON.stringify(this.roles));
        }
      }).add(() => this._progress.end());
  }

  changCheckedAll() {
    this.getRoles();
  }

  changeCheckAllRoles() {
    this.roles.forEach(x => {
      x.isChecked = this.checkAllRoles;
      this.parentChecked(x);
    })
  }

  parentChecked(item: RoleUser) {
    item.actions.filter(x => x.action).forEach(act => act.isChecked = item.isChecked);
  }

  childChecked(role: RoleUser, action: string) {
    if (action == 'Update' || action == 'Delete') {
      let actUpdate = role.actions.find(x => x.action == 'Update');
      let actDelete = role.actions.find(x => x.action == 'Delete');

      let actDetail = role.actions.find(x => x.action == 'GetDetail');
      actDetail.isChecked = actUpdate.isChecked != actDelete.isChecked || actUpdate.isChecked && actDelete.isChecked;
      // actDetail.isDisabled = actUpdate.isChecked != actDelete.isChecked || actUpdate.isChecked && actDelete.isChecked;
    }

    role.isChecked = role.actions.filter(x => x.action).every(item => item.isChecked);
  }

  save() {
    this.role.functionIds = [];

    this.roles.forEach(role => {
      let data = role.actions.filter(act => act.isChecked).map(x => x.id);
      this.role.functionIds.push(...data);
    });
    if (this.role.functionIds.length < 1)
      return this._toast.warning('Warning', 'Bạn phải chọn ít nhất một quyền');

    let method = this.isCreate ? 'create' : 'update';
    let success = `${this.title} nhóm quyền thành công`;
    let error = `${this.title} nhóm quyền có lỗi khi lưu`;

    this.execute(method, success, error);
  }

  execute(method: string, success: string, error: string) {
    this._progress.start();
    this._roleService[method](this.role)
      .subscribe({
        next: (res: OperationResult) => {
          if (res.isSuccess) {
            this._toast.success(CaptionConstants.SUCCESS, success);
            this.back();
          }
          else
            this._toast.error(CaptionConstants.ERROR, res.message ?? error);
        },
        error: () => this._toast.error(CaptionConstants.ERROR, MessageConstants.UN_KNOWN_ERROR),
        complete: () => this._progress.end()
      })
  }

  refresh() {
    if (this.isCreate) {
      this.role = <Role>{
        code: FunctionUtility.generateASCII(8),
        status: true,
        functionIds: []
      };

      this.roles.forEach(x => {
        x.isChecked = false;
        x.actions.forEach(ac => {
          ac.isChecked = false;
          ac.isDisabled = false;
        })
      });
    }
    else {
      this.role = JSON.parse(sessionStorage.getItem('role'));
      this.roles = JSON.parse(sessionStorage.getItem('roles'));
    }

    this.checkAllRoles = false;
  }

  back() {
    this._router.navigate(['admin/role']);
  }
}
