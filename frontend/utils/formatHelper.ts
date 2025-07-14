export const formatCPF = (value: string) => {
    value = value.replace(/\D/g, "").slice(0, 11);
    return value
        .replace(/^(\d{3})(\d)/, "$1.$2")
        .replace(/^(\d{3})\.(\d{3})(\d)/, "$1.$2.$3")
        .replace(/\.(\d{3})(\d)/, ".$1-$2");
};
