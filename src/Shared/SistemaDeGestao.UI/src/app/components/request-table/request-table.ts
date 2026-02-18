import { Component, computed, inject, input, output } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import { DatePipe } from '@angular/common';
import {
  RequestDto,
  RequestStatusLabel,
  RequestCategoryLabel,
  RequestPriorityLabel,
} from '../../models/request.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-request-table',
  standalone: true,
  imports: [MatTableModule, MatIconModule, MatButtonModule, MatTooltipModule, MatChipsModule, DatePipe],
  templateUrl: './request-table.html',
  styleUrl: './request-table.css',
})
export class RequestTableComponent {
  private readonly authService = inject(AuthService);

  readonly isManager = computed(() => this.authService.currentUser()?.role === 'Manager');

  readonly requests = input.required<RequestDto[]>();

  readonly viewHistory = output<RequestDto>();
  readonly approveRequest = output<RequestDto>();
  readonly rejectRequest = output<RequestDto>();

  displayedColumns: string[] = ['title', 'category', 'priority', 'status', 'createdByUserName', 'createdAt', 'actions'];

  statusLabel(value: string): string {
    return RequestStatusLabel[value] ?? value;
  }

  categoryLabel(value: string): string {
    return RequestCategoryLabel[value] ?? value;
  }

  priorityLabel(value: string): string {
    return RequestPriorityLabel[value] ?? value;
  }

  onViewHistory(request: RequestDto): void {
    this.viewHistory.emit(request);
  }

  onApprove(request: RequestDto): void {
    this.approveRequest.emit(request);
  }

  onReject(request: RequestDto): void {
    this.rejectRequest.emit(request);
  }
}
