// models/paginated-result.model.ts
export interface PaginatedResult<T> {
    items: T[];
    totalItems: number;
    pageNumber: number; 
    totalPages: number;
    pageSize: number;
  }