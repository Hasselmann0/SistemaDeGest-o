import { Component, input, output } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Request } from '../../models/request.model';

@Component({
  selector: 'app-request-table',
  standalone: true,
  imports: [MatTableModule, MatIconModule, MatButtonModule, MatTooltipModule],
  templateUrl: './request-table.html',
  styleUrl: './request-table.css',
})
export class RequestTableComponent {
  readonly requests = input.required<Request[]>();

  readonly editRequest = output<Request>();
  readonly deleteRequest = output<Request>();

  displayedColumns: string[] = ['id', 'nome', 'alamat', 'email', 'jenisKelamin', 'actions'];

  onEdit(request: Request): void {
    this.editRequest.emit(request);
  }

  onDelete(request: Request): void {
    this.deleteRequest.emit(request);
  }
}
