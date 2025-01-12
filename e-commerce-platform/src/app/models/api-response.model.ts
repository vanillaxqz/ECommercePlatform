export interface ApiResponse<T> {
    data: T;
    isSuccess: boolean;
    errorMessage: string;
  }