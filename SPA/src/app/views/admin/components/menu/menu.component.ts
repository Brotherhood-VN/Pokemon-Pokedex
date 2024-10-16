import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import {
    CaptionConstants,
    MessageConstants,
} from '@constants/message.constant';
import { Menu } from '@models/admins/menu';
import { MenuService } from '@services/admin/menu.service';
import { FunctionUtility } from '@utilities/function-utility';
import { InjectBase } from '@utilities/inject-base';
import { KeyValuePair } from '@utilities/key-value-utility';
import { OperationResult } from '@utilities/operation-result';
import { MenuItem, TreeDragDropService, TreeNode } from 'primeng/api';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { TreeModule } from 'primeng/tree';
import { IconService } from 'src/app/demo/service/icon.service';

@Component({
    selector: 'app-menu',
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        ButtonModule,
        RippleModule,
        InputTextModule,
        InputNumberModule,
        CheckboxModule,
        TreeModule,
        ContextMenuModule,
        DialogModule,
        DropdownModule,
        BreadcrumbModule,
    ],
    templateUrl: './menu.component.html',
    styleUrl: './menu.component.scss',
    providers: [TreeDragDropService, IconService],
})
export class MenuComponent extends InjectBase implements OnInit {
    // Breadcrumb
    home: MenuItem = <MenuItem>{ icon: 'pi pi-home', routerLink: '/admin' };
    breadcrumbs: MenuItem[] = [{ label: 'Menu' }];

    nodes: TreeNode<Menu>[] = [];
    nodesClone: TreeNode<Menu>[] = [];

    selectedFile: TreeNode<Menu> = <TreeNode<Menu>>{};
    contextItems!: MenuItem[];

    controllers: KeyValuePair[] = [];
    controllerSelect: KeyValuePair = null;

    icons: KeyValuePair[] = [];
    iconSelect: KeyValuePair = null;

    targets: KeyValuePair[] = [
        { key: '_blank', value: '_blank' },
        { key: '_self', value: '_self' },
        { key: '_parent', value: '_parent' },
        { key: '_top', value: '_top' },
        { key: 'framename', value: 'framename' },
    ];

    visible: boolean = false;

    dragdropable: boolean = false;

    menuItem: Menu = <Menu>{};

    titleDialog: string = '';
    isEdit: boolean = false;

    constructor(
        private _menuService: MenuService,
        private _iconService: IconService
    ) {
        super();
    }

    ngOnInit(): void {
        this.contextItems = [
            {
                label: 'Thêm mới',
                icon: 'pi pi-plus',
                command: () => this.addNode(),
            },
            {
                label: 'Chỉnh sửa',
                icon: 'pi pi-pencil',
                command: () => this.editNode(),
            },
            {
                label: 'Xóa',
                icon: 'pi pi-trash',
                command: () => this.removeNode(),
            },
            { separator: true },
            {
                label: 'Bỏ chọn',
                icon: 'pi pi-times',
                command: () => this.unselectFile(),
            },
        ];

        this.loadMenus();
        this.getListController();
        this.getIcons();
    }

    //#region init datas
    loadMenus() {
        this._menuService.loadMenus().subscribe({
            next: (res) => {
                this.nodes = res;
                console.log(this.nodes)
                sessionStorage.setItem('nodes', JSON.stringify(this.nodes));
            },
        });
    }

    getListController() {
        this._menuService.getListController().subscribe({
            next: (res) => {
                this.controllers = res;
            },
        });
    }

    getIcons() {
        this._iconService.getIcons().subscribe((data) => {
            data = data.filter((value) => {
                return value.icon.tags.indexOf('deprecate') === -1;
            });

            let icons = data;
            icons.sort((icon1, icon2) => {
                if (icon1.properties.name < icon2.properties.name) return -1;
                else if (icon1.properties.name < icon2.properties.name)
                    return 1;
                else return 0;
            });

            this.icons = icons.map((icon) => {
                return {
                    key: 'pi-' + icon.properties.name,
                    value: 'pi-' + icon.properties.name,
                };
            });
        });
    }
    //#endregion

    //#region onchange data
    onControllerValueChange(event: KeyValuePair) {
        this.menuItem.controller = '';
        this.menuItem.label = '';
        this.menuItem.routerLink = '';
        if (event) {
            this.menuItem.controller = event.key;
            this.menuItem.label = event.value;
            this.menuItem.routerLink = `admin/${event.key
                .replace(/([a-z])([A-Z])/g, '$1-$2')
                .toLowerCase()}`;
        }
    }

    onIconValueChange(event: KeyValuePair) {
        this.menuItem.icon = '';
        if (event) this.menuItem.icon = 'pi ' + event.key;
    }
    //#endregion

    //#region drag & drop functions
    dragDropable() {
        this.dragdropable = true;
    }

    cancelDragDropable() {
        this.dragdropable = false;
        this.nodes = JSON.parse(sessionStorage.getItem('nodes'));
    }

    saveDragDropable() {
        this._toast.confirm(
            'Are you sure?',
            MessageConstants.CONFIRM_SAVE_MSG,
            () => this.acceptSaveDragDropable(),
            () => this.cancelDragDropable()
        );
    }

    acceptSaveDragDropable() {
        this._progress.start();

        this._menuService
            .configurationMenus(this.nodes)
            .subscribe({
                next: (res: OperationResult) => {
                    if (res.isSuccess) {
                        this._toast.success(
                            CaptionConstants.SUCCESS,
                            'Sắp xếp menus thành công !'
                        );
                        this.dragdropable = false;
                        this.loadMenus();
                    } else
                        this._toast.error(
                            CaptionConstants.ERROR,
                            res.message
                                ? res.message
                                : 'Sắp xếp menus có lỗi khi lưu !'
                        );
                },
                error: () => {
                    this._toast.error(
                        CaptionConstants.ERROR,
                        MessageConstants.UN_KNOWN_ERROR
                    );
                },
            })
            .add(() => this._progress.end());
    }
    //#endregion

    //#region add node
    addNode() {
        if (this.selectedFile && Object.keys(this.selectedFile).length > 0) {
            this.menuItem = <Menu>{
                id: 0,
                parentId: this.selectedFile.data?.id ?? null,
                seq:
                    this.selectedFile.children?.length > 0
                        ? Math.max(
                              ...this.selectedFile.children.map(
                                  (x) => x.data.seq
                              )
                          ) + 1
                        : 1,
                visible: true,
                separator: false,
                badge: '',
                class: '',
                controller: '',
                icon: '',
                items: null,
                label: '',
                routerLink: '',
                target: '',
                title: '',
                url: '',
            };
        } else {
            this.menuItem = <Menu>{
                id: 0,
                seq: 1,
                visible: true,
                separator: false,
                badge: '',
                class: '',
                controller: '',
                icon: '',
                items: null,
                label: '',
                routerLink: '',
                target: '',
                title: '',
                url: '',
            };
        }
        this.visible = true;
        this.titleDialog = 'Thêm mới';
        this.isEdit = false;
    }
    //#endregion

    //#region edit node
    editNode() {
        this.menuItem = this.selectedFile.data;

        this.controllerSelect = this.controllers.find(
            (x) => x.key == this.menuItem.controller
        );
        this.iconSelect = this.icons.find(
            (x) => 'pi ' + x.key == this.menuItem.icon
        );

        this.visible = true;
        this.titleDialog = 'Chỉnh sửa';
        this.isEdit = true;
    }
    //#endregion

    //#region remove node
    removeNode() {
        this._toast.confirm(
            'Are you sure?',
            MessageConstants.CONFIRM_DELETE_MSG,
            () => this.acceptDelete()
        );
    }
    //#endregion

    //#region sub node functions
    unselectFile() {
        this.selectedFile = null;
    }

    toggleNode() {
        this.selectedFile.expanded = !this.selectedFile.expanded;
        this.unselectFile();
    }

    expandAll() {
        this.unselectFile();
        this.nodes.forEach((node) => {
            this.expandRecursive(node, true);
        });
    }

    collapseAll() {
        this.unselectFile();
        this.nodes.forEach((node) => {
            this.expandRecursive(node, false);
        });
    }

    private expandRecursive(node: TreeNode, isExpand: boolean) {
        node.expanded = isExpand;
        if (node.children) {
            node.children.forEach((childNode) => {
                this.expandRecursive(childNode, isExpand);
            });
        }
    }
    //#endregion

    //#region main functions
    save(menuForm: NgForm) {
        let method = !this.isEdit ? 'create' : 'update';
        let success = !this.isEdit
            ? 'Thêm mới menu thành công !'
            : 'Chỉnh sửa menu thành công !';
        let error = !this.isEdit
            ? 'Thêm mới menu có lỗi khi lưu !'
            : 'Chỉnh sửa menu có lỗi khi lưu !';

        this.execute(menuForm, method, success, error);
    }

    execute(menuForm: NgForm, method: string, success: string, error: string) {
        this._progress.start();
        this._menuService[method](this.menuItem)
            .subscribe({
                next: (res: OperationResult) => {
                    if (res.isSuccess) {
                        this._toast.success(CaptionConstants.SUCCESS, success);
                        this.closeDialog(menuForm);
                        this.loadMenus();
                    } else this._toast.success(CaptionConstants.ERROR, error);
                },
                error: () => {
                    this._toast.error(
                        CaptionConstants.ERROR,
                        MessageConstants.UN_KNOWN_ERROR
                    );
                },
            })
            .add(() => this._progress.end());
    }

    closeDialog(menuForm: NgForm) {
        this.visible = false;
        this.menuItem = <Menu>{};
        this.controllerSelect = null;
        this.iconSelect = null;
        menuForm.reset();
    }

    acceptDelete() {
        this._progress.start();

        this._menuService
            .delete(this.selectedFile.data.id)
            .subscribe({
                next: (res: OperationResult) => {
                    if (res.isSuccess) {
                        this._toast.success(
                            CaptionConstants.SUCCESS,
                            `Xóa menu thành công !`
                        );
                        this.loadMenus();
                    } else
                        this._toast.error(
                            CaptionConstants.ERROR,
                            res.message
                                ? res.message
                                : `Xóa menu có lỗi khi lưu !`
                        );
                },
                error: () => {
                    this._toast.error(
                        CaptionConstants.ERROR,
                        MessageConstants.UN_KNOWN_ERROR
                    );
                },
            })
            .add(() => this._progress.end());
    }
    //#endregion
}
