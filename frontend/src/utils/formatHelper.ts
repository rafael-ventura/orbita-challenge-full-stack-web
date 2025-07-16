export const formatCPF = (value: string) => {
  value = value.replace(/\D/g, "").slice(0, 11);
  return value
    .replace(/^(\d{3})(\d)/, "$1.$2")
    .replace(/^(\d{3})\.(\d{3})(\d)/, "$1.$2.$3")
    .replace(/\.(\d{3})(\d)/, ".$1-$2");
};

export const formatRA = (value: string) => {
  return value.replace(/\D/g, "").slice(0, 10);
};

export const formatPhone = (value: string) => {
    if (!value) return '';
    value = value.replace(/\D/g, "").slice(0, 11);
    if (value.length <= 10) {
        return value.replace(/^(\d{2})(\d{4})(\d)/, "($1) $2-$3");
    }
    return value.replace(/^(\d{2})(\d{5})(\d)/, "($1) $2-$3");
};

export const formatDate = (date: string | Date) => {
    if (!date) return '';
    const dateObj = typeof date === 'string' ? new Date(date) : date;
    return dateObj.toLocaleDateString('pt-BR');
};

export const formatCurrency = (value: number) => {
    if (value === null || value === undefined) return '';
    return new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    }).format(value);
}; 