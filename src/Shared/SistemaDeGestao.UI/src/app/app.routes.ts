import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'requests',
    pathMatch: 'full',
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./pages/login-page/login-page').then((m) => m.LoginPageComponent),
  },
  {
    path: 'requests',
    loadComponent: () =>
      import('./pages/request-page/request-page').then((m) => m.RequestPageComponent),
    canActivate: [authGuard],
  },
];
