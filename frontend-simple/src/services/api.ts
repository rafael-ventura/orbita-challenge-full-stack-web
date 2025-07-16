import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios'
import { getApiBaseUrl } from '@/config/app.config'

// Configuração do Axios
const createApiInstance = (): AxiosInstance => {
  const api = axios.create({
    baseURL: getApiBaseUrl(),
    timeout: 30000, // Aumentado para 30 segundos
    headers: {
      'Content-Type': 'application/json',
    }
  })

  // Interceptor para adicionar token automaticamente
  api.interceptors.request.use(
    (config) => {
      const token = localStorage.getItem('token')
      console.log('🔑 Token:', token ? 'Presente' : 'Ausente')
      console.log('🌐 URL Completa:', `${config.baseURL}${config.url}`)
      console.log('📝 Método:', config.method?.toUpperCase())
      console.log('📦 Dados:', config.data)
      
      if (token) {
        config.headers.Authorization = `Bearer ${token}`
      }
      return config
    },
    (error) => {
      console.error('❌ Erro na requisição:', error)
      return Promise.reject(error)
    }
  )

  // Interceptor para tratar respostas e erros
  api.interceptors.response.use(
    (response: AxiosResponse) => {
      return response
    },
    (error) => {
      // Se token expirou ou é inválido, redirecionar para login
      if (error.response?.status === 401) {
        localStorage.removeItem('token')
        localStorage.removeItem('userId')
        localStorage.removeItem('userName')
        localStorage.removeItem('userEmail')
        window.location.href = '/auth/login'
      }
      return Promise.reject(error)
    }
  )

  return api
}

// Instância principal da API
export const api = createApiInstance()

// Funções helpers similares ao $fetch do Nuxt
export const $fetch = {
  get: async <T>(url: string, config?: AxiosRequestConfig): Promise<T> => {
    const response = await api.get<T>(url, config)
    return response.data
  },

  post: async <T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> => {
    const response = await api.post<T>(url, data, config)
    return response.data
  },

  put: async <T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> => {
    const response = await api.put<T>(url, data, config)
    return response.data
  },

  delete: async <T>(url: string, config?: AxiosRequestConfig): Promise<T> => {
    const response = await api.delete<T>(url, config)
    return response.data
  }
}

export default api 