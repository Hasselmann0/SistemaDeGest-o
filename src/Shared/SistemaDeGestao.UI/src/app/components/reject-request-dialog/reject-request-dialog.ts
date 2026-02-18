import { Component, inject, signal } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-reject-request-dialog',
  standalone: true,
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
  ],
  templateUrl: './reject-request-dialog.html',
  styleUrl: './reject-request-dialog.css',
})
export class RejectRequestDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<RejectRequestDialogComponent>);

  comment = '';
  readonly errorMessage = signal('');

  onConfirm(): void {
    if (!this.comment || this.comment.trim().length < 10) {
      this.errorMessage.set('Justificativa deve ter no mínimo 10 caracteres.');
      return;
    }
    this.dialogRef.close(this.comment.trim());
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }
}
