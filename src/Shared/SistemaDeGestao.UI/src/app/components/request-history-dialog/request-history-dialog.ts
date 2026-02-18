import { Component, inject, OnInit, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DatePipe } from '@angular/common';
import { RequestService } from '../../services/request.service';
import {
  RequestDto,
  RequestStatusHistoryDto,
  RequestStatusLabel,
} from '../../models/request.model';

@Component({
  selector: 'app-request-history-dialog',
  standalone: true,
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    DatePipe,
  ],
  templateUrl: './request-history-dialog.html',
  styleUrl: './request-history-dialog.css',
})
export class RequestHistoryDialogComponent implements OnInit {
  private readonly requestService = inject(RequestService);
  private readonly dialogRef = inject(MatDialogRef<RequestHistoryDialogComponent>);
  readonly data: RequestDto = inject(MAT_DIALOG_DATA);

  readonly history = signal<RequestStatusHistoryDto[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');

  ngOnInit(): void {
    this.requestService.getHistory(this.data.id).subscribe({
      next: (data) => {
        this.history.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Erro ao carregar histórico.');
        this.loading.set(false);
      },
    });
  }

  statusLabel(value: string): string {
    return RequestStatusLabel[value] ?? value;
  }

  close(): void {
    this.dialogRef.close();
  }
}
