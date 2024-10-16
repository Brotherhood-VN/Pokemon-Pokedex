import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  // {
  //   path: '',
  //   loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule)
  // },
  {
    path: '',
    loadChildren: () => import('./share/share.module').then(m => m.ShareModule)
  },
  {
    path: 'function',
    loadChildren: () => import('./components/function/function.module').then(m => m.FunctionModule)
  },
  {
    path: 'menu',
    loadComponent: () => import('./components/menu/menu.component').then(c => c.MenuComponent)
  },
  {
    path: 'role',
    loadChildren: () => import('./components/role/role.module').then(m => m.RoleModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
