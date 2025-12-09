import api from './axios';
import type { LoginRequest, RegisterRequest, AuthResponse } from '../../types/auth';

const authService = {
  login: async (data: LoginRequest) => {
    // Anropar POST /api/v1/auth/login
    const response = await api.post<AuthResponse>('/auth/login', data);
    return response.data;
  },

  register: async (data: RegisterRequest) => {
    // Anropar POST /api/v1/auth/register
    const response = await api.post<AuthResponse>('/auth/register', data);
    return response.data;
  },

  getMe: async () => {
    // Anropar GET /api/v1/user/me (eller users/me beroende på vad du valde)
    // OBS: Dubbelkolla din controller-route i backend!
    const response = await api.get('/user/me'); 
    return response.data;
  }
};

export default authService;