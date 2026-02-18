import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  RequestDto,
  RequestDetailDto,
  RequestStatusHistoryDto,
  CreateRequestDto,
  ApproveRequestDto,
  RejectRequestDto,
  RequestFilterParams,
} from '../models/request.model';

@Injectable({
  providedIn: 'root',
})
export class RequestService {
  private readonly http = inject(HttpClient);

  // TODO: Altere para a URL real da sua API
  private readonly apiUrl = 'https://localhost:7041/api/requests';

  /** GET /api/requests — lista todas as solicitações (com filtros opcionais) */
  getAll(filters?: RequestFilterParams): Observable<RequestDto[]> {
    let params = new HttpParams();

    if (filters?.status !== undefined) {
      params = params.set('status', filters.status.toString());
    }
    if (filters?.category !== undefined) {
      params = params.set('category', filters.category.toString());
    }
    if (filters?.priority !== undefined) {
      params = params.set('priority', filters.priority.toString());
    }
    if (filters?.searchText) {
      params = params.set('searchText', filters.searchText);
    }

    return this.http.get<RequestDto[]>(this.apiUrl, { params });
  }

  /** GET /api/requests/{id} — busca detalhes de uma solicitação */
  getById(id: string): Observable<RequestDetailDto> {
    return this.http.get<RequestDetailDto>(`${this.apiUrl}/${id}`);
  }

  /** POST /api/requests — cria uma nova solicitação */
  create(dto: CreateRequestDto): Observable<RequestDetailDto> {
    return this.http.post<RequestDetailDto>(this.apiUrl, dto);
  }

  /** POST /api/requests/{id}/approve — aprova uma solicitação (Manager only) */
  approve(id: string, dto: ApproveRequestDto): Observable<RequestDetailDto> {
    return this.http.post<RequestDetailDto>(`${this.apiUrl}/${id}/approve`, dto);
  }

  /** POST /api/requests/{id}/reject — rejeita uma solicitação (Manager only) */
  reject(id: string, dto: RejectRequestDto): Observable<RequestDetailDto> {
    return this.http.post<RequestDetailDto>(`${this.apiUrl}/${id}/reject`, dto);
  }

  /** GET /api/requests/{id}/history — histórico de status de uma solicitação */
  getHistory(id: string): Observable<RequestStatusHistoryDto[]> {
    return this.http.get<RequestStatusHistoryDto[]>(`${this.apiUrl}/${id}/history`);
  }
}
