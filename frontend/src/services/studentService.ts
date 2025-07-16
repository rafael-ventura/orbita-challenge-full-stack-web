import { api } from './api'
import type { Student, CreateStudentRequest, UpdateStudentRequest } from '@/types/student'

export class StudentService {
  static async getAllStudents(): Promise<Student[]> {
    try {
      const response = await api.get('/Student')
      
      // Se a resposta for um array vazio, retornar array vazio (não é erro)
      if (Array.isArray(response.data)) {
        return response.data
      }
      
      // Se a resposta não for um array, retornar array vazio
      return []
    } catch (error: any) {
      console.error('Erro detalhado:', error.response?.data || error.message)
      
      // Se o status for 404 (não encontrado), pode ser que não há estudantes ainda
      if (error.response?.status === 404) {
        return []
      }
      
      if (error.response?.status === 401) {
        throw new Error('Usuário não autenticado. Faça login novamente.')
      }
      
      if (error.response?.status === 500) {
        throw new Error('Erro interno do servidor. Tente novamente mais tarde.')
      }
      
      // Para outros erros de rede ou servidor
      if (error.code === 'NETWORK_ERROR' || error.code === 'ECONNABORTED') {
        throw new Error('Erro de conexão. Verifique sua internet e tente novamente.')
      }
      
      throw new Error(`Erro ao buscar estudantes: ${error.response?.data?.message || error.message}`)
    }
  }

  static async getStudentById(id: string): Promise<Student> {
    try {
      const response = await api.get(`/Student/${id}`)
      return response.data
    } catch (error: any) {
      console.error('Erro detalhado:', error.response?.data || error.message)
      
      if (error.response?.status === 401) {
        throw new Error('Usuário não autenticado. Faça login novamente.')
      }
      
      if (error.response?.status === 404) {
        throw new Error('Estudante não encontrado.')
      }
      
      throw new Error(`Erro ao buscar estudante: ${error.response?.data?.message || error.message}`)
    }
  }

  static async createStudent(studentData: CreateStudentRequest): Promise<Student> {
    try {
      const response = await api.post('/Student', studentData)
      return response.data
    } catch (error: any) {
      console.error('Erro detalhado:', error.response?.data || error.message)
      
      if (error.response?.status === 401) {
        throw new Error('Usuário não autenticado. Faça login novamente.')
      }
      
      // Propagar o erro completo para o componente tratar mensagens detalhadas
      throw error
    }
  }

  static async updateStudent(id: string, studentData: UpdateStudentRequest): Promise<Student> {
    try {
      const response = await api.put(`/Student/${id}`, studentData)
      return response.data
    } catch (error: any) {
      console.error('Erro detalhado:', error.response?.data || error.message)
      
      if (error.response?.status === 401) {
        throw new Error('Usuário não autenticado. Faça login novamente.')
      }
      
      if (error.response?.status === 400) {
        const message = error.response.data?.message || 'Dados inválidos'
        throw new Error(message)
      }
      
      if (error.response?.status === 404) {
        throw new Error('Estudante não encontrado.')
      }
      
      throw new Error(`Erro ao atualizar estudante: ${error.response?.data?.message || error.message}`)
    }
  }

  static async deleteStudent(id: string): Promise<void> {
    try {
      await api.delete(`/Student/${id}`)
    } catch (error: any) {
      console.error('Erro detalhado:', error.response?.data || error.message)
      
      if (error.response?.status === 401) {
        throw new Error('Usuário não autenticado. Faça login novamente.')
      }
      
      if (error.response?.status === 404) {
        throw new Error('Estudante não encontrado.')
      }
      
      if (error.response?.status === 500) {
        throw new Error('Erro interno do servidor. Tente novamente mais tarde.')
      }
      
      // Para outros erros de rede ou servidor
      if (error.code === 'NETWORK_ERROR' || error.code === 'ECONNABORTED') {
        throw new Error('Erro de conexão. Verifique sua internet e tente novamente.')
      }
      
      throw new Error(`Erro ao deletar estudante: ${error.response?.data?.message || error.message}`)
    }
  }

  static async searchStudents(query: string): Promise<Student[]> {
    try {
      const response = await api.get(`/Student/search?q=${encodeURIComponent(query)}`)
      
      // Se a resposta for um array vazio, retornar array vazio (não é erro)
      if (Array.isArray(response.data)) {
        return response.data
      }
      
      // Se a resposta não for um array, retornar array vazio
      return []
    } catch (error: any) {
      console.error('Erro detalhado:', error.response?.data || error.message)
      
      // Se o status for 404 (não encontrado), pode ser que não há resultados
      if (error.response?.status === 404) {
        return []
      }
      
      if (error.response?.status === 401) {
        throw new Error('Usuário não autenticado. Faça login novamente.')
      }
      
      if (error.response?.status === 500) {
        throw new Error('Erro interno do servidor. Tente novamente mais tarde.')
      }
      
      // Para outros erros de rede ou servidor
      if (error.code === 'NETWORK_ERROR' || error.code === 'ECONNABORTED') {
        throw new Error('Erro de conexão. Verifique sua internet e tente novamente.')
      }
      
      throw new Error(`Erro ao buscar estudantes: ${error.response?.data?.message || error.message}`)
    }
  }
} 