import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GeneralLayoutComponent } from './general-layout/general-layout.component';
import { AuthConstants } from '@constants/auth.constant';
import { Menu } from '@models/admins/menu';

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

let menus = JSON.parse(localStorage.getItem(AuthConstants.MENUS)) as Menu[];

let routes: Routes = [];

// controllers.forEach(controller => {
//   let menu = menus.find(x => x.controller == controller);
//   if (menu) {
//     routes.push(
//       {
//         path: menu.routerLink ?? controller.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase(),
//         component: GeneralLayoutComponent,
//         data: {
//           'title': menu.title,
//           'controller': controller
//         }
//       }
//     );
//     console.log(routes)
//   }
// });
routes.push(
  {
    path: 'AccountType',
    component: GeneralLayoutComponent,
    data: {
      'title': "AccountType",
      'controller': "AccountType"
    }
  },
  {
    path: 'Area',
    component: GeneralLayoutComponent,
    data: {
      'title': "Area",
      'controller': "Area"
    }
  },
  {
    path: 'Condition',
    component: GeneralLayoutComponent,
    data: {
      'title': "Condition",
      'controller': "Condition"
    }
  },
  {
    path: 'GameVersion',
    component: GeneralLayoutComponent,
    data: {
      'title': "GameVersion",
      'controller': "GameVersion"
    }
  },
  {
    path: 'Item',
    component: GeneralLayoutComponent,
    data: {
      'title': "Item",
      'controller': "Item"
    }
  },
  {
    path: 'Generation',
    component: GeneralLayoutComponent,
    data: {
      'title': "Generation",
      'controller': "Generation"
    }
  },
  {
    path: 'Rank',
    component: GeneralLayoutComponent,
    data: {
      'title': "Rank",
      'controller': "Rank"
    }
  },
  {
    path: 'StatType',
    component: GeneralLayoutComponent,
    data: {
      'title': "StatType",
      'controller': "StatType"
    }
  },
  {
    path: 'Stone',
    component: GeneralLayoutComponent,
    data: {
      'title': "Stone",
      'controller': "Stone"
    }
  },
  {
    path: 'Gender',
    component: GeneralLayoutComponent,
    data: {
      'title': "Gender",
      'controller': "Gender"
    }
  },
  {
    path: 'Region',
    component: GeneralLayoutComponent,
    data: {
      'title': "Region",
      'controller': "Region"
    }
  },
);
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShareRoutingModule { }
