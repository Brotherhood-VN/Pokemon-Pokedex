import { Component, OnInit } from '@angular/core';
import { CaptionConstants, MessageConstants } from '@constants/message.constant';
import { Role } from '@models/admins/role';
import { RoleService } from '@services/admin/role.service';
import { InjectBase } from '@utilities/inject-base';
import { Pagination } from '@utilities/pagination-utility';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent extends InjectBase implements OnInit {
  // Breadcrumb
  home: MenuItem = <MenuItem>{ icon: 'pi pi-home', routerLink: '/admin' };
  breadcrumbs: MenuItem[] = [{ label: 'Role' }];

  roles: Role[] = [];
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0
  }
  keyword: string = '';

  constructor(private _roleService: RoleService) {
    super();
  }

  ngOnInit() {
    this.search();
  }

  getDataPagination() {
    this._progress.start();

    this._roleService.getDataPagination(this.pagination, this.keyword)
      .subscribe({
        next: (res) => {
          this.roles = res.result;
          this.pagination = res.pagination;
        }
      }).add(() => this._progress.end());
  }

  search() {
    this.pagination.pageNumber = 1;
    this.getDataPagination();
  }

  clearSearch() {
    this.keyword = '';
    this.search();
  }

  onPageChange(event: any) {
    this.pagination.pageNumber = event.page + 1;
    this.pagination.pageSize = event.rows;
    this.getDataPagination();
  }

  create() {
    this._router.navigate(['admin/role/create']);
  }

  update(id: string) {
    this._router.navigate(['admin/role/update'], { state: { 'id': id } });
  }

  delete(role: Role) {
    this._toast.confirm('Are you sure?', 'Bạn có chắc muốn lưu dữ liệu này', () => this.acceptDelete(role));
  }

  acceptDelete(role: Role) {
    this._progress.start();
    this._roleService.delete(role)
      .subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this._toast.success(CaptionConstants.SUCCESS, 'Xóa nhóm quyền thành công !');
            this.search();
          }
          else
            this._toast.error(CaptionConstants.ERROR, res.message ?? 'Xóa nhóm quyền có lỗi khi lưu !');
        },
        error: () => this._toast.error(CaptionConstants.ERROR, MessageConstants.UN_KNOWN_ERROR),
        complete: () => this._progress.end()
      })
  }
}
