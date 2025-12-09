export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  // profilePictureUrl?: string; // Vi kan lägga till denna senare
}

export interface AuthResponse {
  success: boolean;
  data: {
    token: string;
    refreshToken: string;
    expiresAt: string;
    user?: User; // Ibland skickar vi med user direkt vid login, ibland hämtar vi den separat
  };
  errors?: string[];
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}