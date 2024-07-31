import { NgModule, Provider } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ShareRoutingModule } from './share-routing.module';
import { GeneralLayoutComponent } from './general-layout/general-layout.component';
import { QuillModule } from 'ngx-quill';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { PaginatorModule } from 'primeng/paginator';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { SliderModule } from 'primeng/slider';
import { MultiSelectModule } from 'primeng/multiselect';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { BaseModel } from '@models/admins/base-model';
import { BaseService } from '@services/admin/_base.service';
import { QuillEditorComponent } from './quill-editor/quill-editor.component';

const controllers: string[] = [
  "AccountType",
  "Area",
  "Condition",
  "GameVersion",
  "Gender",
  "Generation",
  "Item",
  "Rank",
  "StatType",
  "Stone",
]
let providers: Provider[] = [];
controllers.forEach(controller => {
  providers.push({ provide: BaseService<BaseModel>, useFactory: () => new BaseService<BaseModel>(controller) })
})

@NgModule({
  declarations: [
    GeneralLayoutComponent,
    QuillEditorComponent
  ],
  imports: [
    CommonModule,
    ShareRoutingModule,
    FormsModule,
    TableModule,
    PaginatorModule,
    ButtonModule,
    InputTextModule,
    RippleModule,
    SliderModule,
    MultiSelectModule,
    CalendarModule,
    CheckboxModule,
    BreadcrumbModule,
    QuillModule.forRoot()
  ],
  providers: providers,
  exports: [
    QuillEditorComponent
  ]
})
export class ShareModule { }
