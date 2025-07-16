export const validationRules = {
  required: (v: string) => !!v || "Este campo é obrigatório.",
  numeric: (v: string) => /^\d+$/.test(v) || "Somente números são permitidos.",
  cpf: (v: string) => /^\d{3}\.\d{3}\.\d{3}-\d{2}$/.test(v) || "CPF deve conter 11 números.",
  email: (v: string) =>
    /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{1,}$/.test(v) || "E-mail inválido.",
  minLength: (min: number) => (v: string) => 
    v.length >= min || `Mínimo de ${min} caracteres.`,
  maxLength: (max: number) => (v: string) => 
    v.length <= max || `Máximo de ${max} caracteres.`,
};

// Função para validar formulários completos
export const validateForm = (data: Record<string, any>, rules: Record<string, any[]>) => {
    const errors: Record<string, string> = {}
    
    for (const [field, fieldRules] of Object.entries(rules)) {
        const value = data[field]
        
        for (const rule of fieldRules) {
            const result = rule(value)
            if (result !== true) {
                errors[field] = result
                break // Para na primeira regra que falha
            }
        }
    }
    
    return {
        isValid: Object.keys(errors).length === 0,
        errors
    }
} 