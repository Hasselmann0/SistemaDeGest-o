import { Component, inject } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-approve-request-dialog',
  standalone: true,
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
  ],
  templateUrl: './approve-request-dialog.html',
  styleUrl: './approve-request-dialog.css',
})
export class ApproveRequestDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<ApproveRequestDialogComponent>);

  comment = '';

  onConfirm(): void {
    this.dialogRef.close({ confirmed: true, comment: this.comment.trim() });
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }
}
