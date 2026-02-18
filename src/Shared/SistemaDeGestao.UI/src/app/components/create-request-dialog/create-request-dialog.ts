import { Component, inject, signal } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule } from '@angular/forms';
import {
  CreateRequestDto,
  RequestCategory,
  RequestPriority,
} from '../../models/request.model';
import { RequestService } from '../../services/request.service';

@Component({
  selector: 'app-create-request-dialog',
  standalone: true,
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    FormsModule,
  ],
  templateUrl: './create-request-dialog.html',
  styleUrl: './create-request-dialog.css',
})
export class CreateRequestDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<CreateRequestDialogComponent>);
  private readonly requestService = inject(RequestService);

  title = '';
  description = '';
  category: RequestCategory | null = null;
  priority: RequestPriority | null = null;

  readonly loading = signal(false);
  readonly errorMessage = signal('');

  readonly categories = [
    { value: RequestCategory.Compras, label: 'Compras' },
    { value: RequestCategory.TI, label: 'TI' },
    { value: RequestCategory.Reembolso, label: 'Reembolso' },
  ];

  readonly priorities = [
    { value: RequestPriority.Baixa, label: 'Baixa' },
    { value: RequestPriority.Media, label: 'Média' },
    { value: RequestPriority.Alta, label: 'Alta' },
  ];

  onSubmit(): void {
    if (!this.title || !this.description || this.category === null || this.priority === null) {
      this.errorMessage.set('Preencha todos os campos.');
      return;
    }

    if (this.title.length < 3) {
      this.errorMessage.set('Título deve ter no mínimo 3 caracteres.');
      return;
    }

    if (this.description.length < 10) {
      this.errorMessage.set('Descrição deve ter no mínimo 10 caracteres.');
      return;
    }

    this.loading.set(true);
    this.errorMessage.set('');

    const dto: CreateRequestDto = {
      title: this.title,
      description: this.description,
      category: this.category,
      priority: this.priority,
    };

    this.requestService.create(dto).subscribe({
      next: (result) => {
        this.loading.set(false);
        this.dialogRef.close(result);
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(err.error?.message ?? 'Erro ao criar solicitação.');
      },
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
