import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [
    FormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPageComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  email = '';
  password = '';
  hidePassword = true;

  readonly loading = signal(false);
  readonly errorMessage = signal('');

  onLogin(): void {
    if (!this.email || !this.password) {
      this.errorMessage.set('Preencha todos os campos.');
      return;
    }

    this.loading.set(true);
    this.errorMessage.set('');

    this.authService
      .login({ email: this.email, password: this.password })
      .subscribe({
        next: () => {
          this.loading.set(false);
          this.router.navigate(['/requests']);
        },
        error: (err) => {
          this.loading.set(false);
          if (err.status === 401) {
            this.errorMessage.set('Email ou senha inválidos.');
          } else {
            this.errorMessage.set('Erro ao conectar com o servidor.');
          }
        },
      });
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }
}
