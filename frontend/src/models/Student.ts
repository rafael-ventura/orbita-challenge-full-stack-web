export class Student {
  id?: string;
  name: string;
  email: string;
  ra: string;
  cpf: string;
  createdAt?: Date;
  updatedAt?: Date;

  constructor(name = "", email = "", ra = "", cpf = "") {
    this.name = name;
    this.email = email;
    this.ra = ra;
    this.cpf = cpf;
  }
}

// Interface para resposta de autenticação
export interface AuthResponse {
    id: number;
    name: string;
    email: string;
    token: string;
}

// Interface para dados de usuário
export interface User {
    id: number;
    name: string;
    email: string;
}

// Interface para credenciais de login
export interface LoginCredentials {
    email: string;
    password: string;
}

// Interface para dados de registro
export interface RegisterData {
    name: string;
    email: string;
    password: string;
} 