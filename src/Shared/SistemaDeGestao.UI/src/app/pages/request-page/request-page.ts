import { Component, inject, OnInit, signal } from '@angular/core';
import { MatFabButton } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { RequestTableComponent } from '../../components/request-table/request-table';
import { RequestService } from '../../services/request.service';
import { RequestDto } from '../../models/request.model';
import { RequestHistoryDialogComponent } from '../../components/request-history-dialog/request-history-dialog';
import { CreateRequestDialogComponent } from '../../components/create-request-dialog/create-request-dialog';

@Component({
  selector: 'app-request-page',
  standalone: true,
  imports: [
    RequestTableComponent,
    MatFabButton,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDialogModule,
  ],
  templateUrl: './request-page.html',
  styleUrl: './request-page.css',
})
export class RequestPageComponent implements OnInit {
  private readonly requestService = inject(RequestService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);

  readonly requests = signal<RequestDto[]>([]);
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
    const dialogRef = this.dialog.open(CreateRequestDialogComponent, {
      width: '560px',
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.snackBar.open('Solicitação criada com sucesso!', 'Fechar', { duration: 3000 });
        this.loadRequests();
      }
    });
  }

  onViewHistory(request: RequestDto): void {
    this.dialog.open(RequestHistoryDialogComponent, {
      width: '520px',
      data: request,
    });
  }

  onApproveRequest(request: RequestDto): void {
    this.requestService.approve(request.id, {}).subscribe({
      next: () => {
        this.snackBar.open('Solicitação aprovada com sucesso!', 'Fechar', { duration: 3000 });
        this.loadRequests();
      },
      error: (err) => {
        console.error('Erro ao aprovar solicitação:', err);
        this.snackBar.open(
          err.error?.message ?? 'Erro ao aprovar solicitação.',
          'Fechar',
          { duration: 5000 }
        );
      },
    });
  }

  onRejectRequest(request: RequestDto): void {
    const comment = prompt('Justificativa para rejeição (mín. 10 caracteres):');
    if (!comment || comment.length < 10) {
      this.snackBar.open('Justificativa deve ter no mínimo 10 caracteres.', 'Fechar', { duration: 4000 });
      return;
    }

    this.requestService.reject(request.id, { comment }).subscribe({
      next: () => {
        this.snackBar.open('Solicitação rejeitada.', 'Fechar', { duration: 3000 });
        this.loadRequests();
      },
      error: (err) => {
        console.error('Erro ao rejeitar solicitação:', err);
        this.snackBar.open(
          err.error?.message ?? 'Erro ao rejeitar solicitação.',
          'Fechar',
          { duration: 5000 }
        );
      },
    });
  }
}
