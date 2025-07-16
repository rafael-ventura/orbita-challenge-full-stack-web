export interface Student {
  id: string
  name: string
  email: string
  cpf: string
  ra: string
  createdAt: string
  updatedAt: string
}

export interface CreateStudentRequest {
  Name: string
  Email: string
  CPF: string
  RA: string
}

export interface UpdateStudentRequest {
  Name?: string
  Email?: string
  CPF?: string
  RA?: string
} 