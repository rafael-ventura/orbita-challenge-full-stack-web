import { api } from './api'
import type { Student, CreateStudentRequest, UpdateStudentRequest } from '@/types/student'

export class StudentService {
  static async getAllStudents(): Promise<Student[]> {
    try {
      const response = await api.get('/Student')
      return response.data
    } catch (error) {
      throw new Error('Erro ao buscar estudantes')
    }
  }

  static async getStudentById(id: string): Promise<Student> {
    try {
      const response = await api.get(`/Student/${id}`)
      return response.data
    } catch (error) {
      throw new Error('Erro ao buscar estudante')
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
      
      if (error.response?.status === 400) {
        const message = error.response.data?.message || 'Dados inválidos'
        throw new Error(message)
      }
      
      if (error.response?.status === 404) {
        throw new Error('Endpoint não encontrado. Verifique se o backend está rodando.')
      }
      
      throw new Error(`Erro ao criar estudante: ${error.response?.data?.message || error.message}`)
    }
  }

  static async updateStudent(id: string, studentData: UpdateStudentRequest): Promise<Student> {
    try {
      const response = await api.put(`/Student/${id}`, studentData)
      return response.data
    } catch (error) {
      throw new Error('Erro ao atualizar estudante')
    }
  }

  static async deleteStudent(id: string): Promise<void> {
    try {
      await api.delete(`/Student/${id}`)
    } catch (error) {
      throw new Error('Erro ao deletar estudante')
    }
  }

  static async searchStudents(query: string): Promise<Student[]> {
    try {
      const response = await api.get(`/Student/search?q=${encodeURIComponent(query)}`)
      return response.data
    } catch (error) {
      throw new Error('Erro ao buscar estudantes')
    }
  }
} 