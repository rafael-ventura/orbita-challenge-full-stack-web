// Configuração centralizada da aplicação
export const appConfig = {
  api: {
    baseUrl: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5264/api'
  },
  app: {
    name: 'Student Management',
    version: '1.0.0'
  }
}

// Função helper para acessar configurações
export const getApiBaseUrl = () => {
  const baseUrl = appConfig.api.baseUrl
  console.log('API Base URL:', baseUrl)
  return baseUrl
} 