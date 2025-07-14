export const validationRules = {
    required: (v: string) => !!v || "Este campo é obrigatório.",
    numeric: (v: string) => /^\d+$/.test(v) || "Somente números são permitidos.",
    cpf: (v: string) => /^\d{3}\.\d{3}\.\d{3}-\d{2}$/.test(v) || "CPF deve conter 11 números.",
    email: (v: string) =>
        /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{1,}$/.test(v) || "E-mail inválido.",
};
