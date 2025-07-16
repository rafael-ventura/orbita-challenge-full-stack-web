export const getErrorMessage = (internalCode: string, defaultMessage: string): string => {
    const errorMessages: Record<string, string> = {
        'VALIDATION_FAILED': 'Erro de validação. Verifique os dados informados.',
        'STUDENT_NOT_FOUND': 'Estudante não encontrado.',
        'EMAIL_ALREADY_EXISTS': 'Este e-mail já está cadastrado.',
        'CPF_ALREADY_EXISTS': 'Este CPF já está cadastrado.',
        'RA_ALREADY_EXISTS': 'Este RA já está cadastrado.',
        'INVALID_CREDENTIALS': 'Credenciais inválidas.',
        'TOKEN_EXPIRED': 'Sessão expirada. Faça login novamente.',
        'UNAUTHORIZED': 'Acesso não autorizado.',
        'SERVER_ERROR': 'Erro interno do servidor.',
        'NETWORK_ERROR': 'Erro de conexão. Verifique sua internet.',
    }

    return errorMessages[internalCode] || defaultMessage || 'Erro desconhecido.'
}

export const getFieldErrorMessage = (field: string, error: string): string => {
    const fieldMessages: Record<string, Record<string, string>> = {
        email: {
            required: 'E-mail é obrigatório',
            invalid: 'E-mail inválido'
        },
        password: {
            required: 'Senha é obrigatória',
            minLength: 'Senha deve ter pelo menos 6 caracteres'
        },
        name: {
            required: 'Nome é obrigatório',
            minLength: 'Nome deve ter pelo menos 2 caracteres'
        },
        cpf: {
            required: 'CPF é obrigatório',
            invalid: 'CPF inválido'
        },
        ra: {
            required: 'RA é obrigatório',
            invalid: 'RA inválido'
        }
    }

    return fieldMessages[field]?.[error] || error
} 