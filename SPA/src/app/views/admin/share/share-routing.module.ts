import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GeneralLayoutComponent } from './general-layout/general-layout.component';
import { AuthConstants } from '@constants/auth.constant';
import { Menu } from '@models/admins/menu';

const controllers: string[] = [
  "AccountType",
  "Classification",
  "Condition",
  "Gender",
  "Generation",
  "Stone",
]

let menus = JSON.parse(localStorage.getItem(AuthConstants.MENUS)) as Menu[];

let routes: Routes = [];

controllers.forEach(controller => {
  let menu = menus.find(x => x.controller == controller);
  if (menu) {
    routes.push(
      {
        path: menu.routerLink ?? controller.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase(),
        component: GeneralLayoutComponent,
        data: {
          'title': menu.title,
          'controller': controller
        }
      }
    );
  }
});

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShareRoutingModule { }
