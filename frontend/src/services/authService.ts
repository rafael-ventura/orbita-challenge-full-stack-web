import { api } from './api'
import type { LoginRequest, RegisterRequest, AuthResponse } from '@/types/auth'

export class AuthService {
  static async login(credentials: LoginRequest): Promise<AuthResponse> {
    try {
      const response = await api.post('/auth/login', credentials)
      return response.data
    } catch (error) {
      throw new Error('Erro ao fazer login')
    }
  }

  static async register(userData: RegisterRequest): Promise<AuthResponse> {
    try {
      const response = await api.post('/auth/register', userData)
      return response.data
    } catch (error) {
      throw new Error('Erro ao registrar usuário')
    }
  }

  static async logout(): Promise<void> {
    try {
      // Tentar fazer logout no backend (opcional)
      await api.post('/auth/logout')
    } catch (error) {
      console.error('Erro ao fazer logout no backend:', error)
    } finally {
      // Sempre limpar dados locais
      this.clearAuthData()
    }
  }

  static clearAuthData(): void {
    localStorage.removeItem('token')
    localStorage.removeItem('userId')
    localStorage.removeItem('userName')
    localStorage.removeItem('userEmail')
    localStorage.removeItem('user')
  }

  static isAuthenticated(): boolean {
    const token = localStorage.getItem('token')
    return !!token
  }

  static getToken(): string | null {
    return localStorage.getItem('token')
  }

  static getUser(): any {
    const user = localStorage.getItem('user')
    return user ? JSON.parse(user) : null
  }

  static setAuthData(token: string, user: any): void {
    localStorage.setItem('token', token)
    localStorage.setItem('user', JSON.stringify(user))
    
    // Salvar dados individuais para fácil acesso
    if (user) {
      localStorage.setItem('userId', user.id?.toString() || '')
      localStorage.setItem('userName', user.name || '')
      localStorage.setItem('userEmail', user.email || '')
    }
  }
} 