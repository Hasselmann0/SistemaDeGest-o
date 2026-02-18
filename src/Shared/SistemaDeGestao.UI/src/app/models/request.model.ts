// ── Enums (espelham os enums do backend) ──

export enum RequestStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2,
}

export enum RequestCategory {
  Compras = 0,
  TI = 1,
  Reembolso = 2,
}

export enum RequestPriority {
  Baixa = 0,
  Media = 1,
  Alta = 2,
}

// ── Labels para exibição ──

export const RequestStatusLabel: Record<string, string> = {
  Pending: 'Pendente',
  Approved: 'Aprovado',
  Rejected: 'Rejeitado',
};

export const RequestCategoryLabel: Record<string, string> = {
  Compras: 'Compras',
  TI: 'TI',
  Reembolso: 'Reembolso',
};

export const RequestPriorityLabel: Record<string, string> = {
  Baixa: 'Baixa',
  Media: 'Média',
  Alta: 'Alta',
};

// ── DTOs de Response ──

/** GET /api/requests → RequestDto[] */
export interface RequestDto {
  id: string;
  title: string;
  description: string;
  category: string;
  priority: string;
  status: string;
  createdByUserId: string;
  createdByUserName: string;
  createdAt: string;
  updatedAt: string | null;
}

/** GET /api/requests/{id} → RequestDetailDto */
export interface RequestDetailDto extends RequestDto {
  statusHistory: RequestStatusHistoryDto[];
}

/** GET /api/requests/{id}/history → RequestStatusHistoryDto[] */
export interface RequestStatusHistoryDto {
  id: string;
  fromStatus: string;
  toStatus: string;
  changedByUserId: string;
  changedByUserName: string;
  comment: string | null;
  changedAt: string;
}

// ── DTOs de Request (body) ──

/** POST /api/requests */
export interface CreateRequestDto {
  title: string;
  description: string;
  category: RequestCategory;
  priority: RequestPriority;
}

/** POST /api/requests/{id}/approve (Manager only) */
export interface ApproveRequestDto {
  comment?: string;
}

/** POST /api/requests/{id}/reject (Manager only) */
export interface RejectRequestDto {
  comment: string;
}

/** Query params para GET /api/requests */
export interface RequestFilterParams {
  status?: RequestStatus;
  category?: RequestCategory;
  priority?: RequestPriority;
  searchText?: string;
}
