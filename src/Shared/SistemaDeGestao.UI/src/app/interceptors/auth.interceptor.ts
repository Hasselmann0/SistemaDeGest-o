import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const token = authService.getToken();

  // Não adiciona token em requisições de login
  if (req.url.includes('/auth/login')) {
    return next(req);
  }

  if (token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });

    return next(cloned).pipe(
      catchError((error) => {
        if (error.status === 401 || error.status === 403) {
          authService.logout();
        }
        return throwError(() => error);
      })
    );
  }

  return next(req);
};
