import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Request } from '../models/request.model';

@Injectable({
  providedIn: 'root',
})
export class RequestService {
  private readonly http = inject(HttpClient);

  // TODO: Altere para a URL real da sua API
  private readonly apiUrl = 'https://localhost:7041/api/requests';

  getAll(): Observable<Request[]> {
    return this.http.get<Request[]>(this.apiUrl);
  }

  getById(id: number): Observable<Request> {
    return this.http.get<Request>(`${this.apiUrl}/${id}`);
  }

  create(request: Omit<Request, 'id'>): Observable<Request> {
    return this.http.post<Request>(this.apiUrl, request);
  }

  update(id: number, request: Request): Observable<Request> {
    return this.http.put<Request>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
