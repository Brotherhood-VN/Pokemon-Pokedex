import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FunctionRoutingModule } from './function-routing.module';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { FormGroupComponent } from './form-group/form-group.component';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DropdownModule } from 'primeng/dropdown';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { TableModule } from 'primeng/table';
import { CheckboxModule } from 'primeng/checkbox';
import { PaginatorModule } from 'primeng/paginator';
import { DialogService, DynamicDialogModule } from 'primeng/dynamicdialog';

@NgModule({
  declarations: [
    MainComponent,
    FormComponent,
    FormGroupComponent
  ],
  imports: [
    CommonModule,
    FunctionRoutingModule,
    FormsModule,
    ButtonModule,
    RippleModule,
    InputTextModule,
    InputNumberModule,
    ContextMenuModule,
    DynamicDialogModule,
    DropdownModule,
    BreadcrumbModule,
    TableModule,
    CheckboxModule,
    PaginatorModule
  ],
  providers: [DialogService]
})
export class FunctionModule { }
