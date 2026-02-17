import { Component, inject, OnInit, signal } from '@angular/core';
import { MatFabButton } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { RequestTableComponent } from '../../components/request-table/request-table';
import { RequestService } from '../../services/request.service';
import { Request } from '../../models/request.model';

@Component({
  selector: 'app-request-page',
  standalone: true,
  imports: [
    RequestTableComponent,
    MatFabButton,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
  ],
  templateUrl: './request-page.html',
  styleUrl: './request-page.css',
})
export class RequestPageComponent implements OnInit {
  private readonly requestService = inject(RequestService);
  private readonly snackBar = inject(MatSnackBar);

  readonly requests = signal<Request[]>([]);
  readonly loading = signal(false);

  ngOnInit(): void {
    this.loadRequests();
  }

  loadRequests(): void {
    this.loading.set(true);
    this.requestService.getAll().subscribe({
      next: (data) => {
        this.requests.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Erro ao carregar solicitações:', err);
        this.snackBar.open('Erro ao carregar solicitações. Verifique a conexão com a API.', 'Fechar', {
          duration: 5000,
        });
        this.loading.set(false);
      },
    });
  }

  onAddRequest(): void {
    // TODO: Abrir dialog de criação de request
    console.log('Adicionar nova solicitação');
  }

  onEditRequest(request: Request): void {
    // TODO: Abrir dialog de edição
    console.log('Editar solicitação:', request);
  }

  onDeleteRequest(request: Request): void {
    if (confirm(`Deseja realmente excluir a solicitação "${request.nome}"?`)) {
      this.requestService.delete(request.id).subscribe({
        next: () => {
          this.snackBar.open('Solicitação excluída com sucesso!', 'Fechar', { duration: 3000 });
          this.loadRequests();
        },
        error: (err) => {
          console.error('Erro ao excluir solicitação:', err);
          this.snackBar.open('Erro ao excluir solicitação.', 'Fechar', { duration: 5000 });
        },
      });
    }
  }
}
