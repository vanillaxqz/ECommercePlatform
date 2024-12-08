import { User } from './user.model';

export interface AuthResponse {
  data: {
    user: User;
    token: string;
  };
  isSuccess: boolean;
  errorMessage?: string;
}