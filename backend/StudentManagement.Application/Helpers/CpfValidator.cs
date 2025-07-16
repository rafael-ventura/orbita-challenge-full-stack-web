using System.Text.RegularExpressions;

namespace StudentManagement.Application.Helpers;

public static class CpfValidator
{
    /// <summary>
    /// Valida se o CPF é válido usando o algoritmo oficial
    /// </summary>
    /// <param name="cpf">CPF a ser validado</param>
    /// <returns>True se o CPF é válido</returns>
    public static bool IsValid(string cpf)
    {
        // Remove caracteres não numéricos
        cpf = Regex.Replace(cpf, @"[^\d]", "");
        
        // Verifica se tem 11 dígitos
        if (cpf.Length != 11)
            return false;
        
        // Verifica se todos os dígitos são iguais (CPF inválido)
        if (cpf.All(c => c == cpf[0]))
            return false;
        
        // Calcula o primeiro dígito verificador
        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        
        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;
        
        // Calcula o segundo dígito verificador
        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        
        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;
        
        // Verifica se os dígitos calculados são iguais aos do CPF
        return int.Parse(cpf[9].ToString()) == digit1 && 
               int.Parse(cpf[10].ToString()) == digit2;
    }
    
    /// <summary>
    /// Formata o CPF no padrão XXX.XXX.XXX-XX
    /// </summary>
    /// <param name="cpf">CPF sem formatação</param>
    /// <returns>CPF formatado</returns>
    public static string Format(string cpf)
    {
        var normalized = Regex.Replace(cpf, @"[^\d]", "");
        
        if (normalized.Length != 11)
            return cpf; // Retorna o original se não conseguir normalizar
        
        return $"{normalized[..3]}.{normalized.Substring(3, 3)}.{normalized.Substring(6, 3)}-{normalized.Substring(9, 2)}";
    }
    
    /// <summary>
    /// Remove formatação do CPF, deixando apenas números
    /// </summary>
    /// <param name="cpf">CPF com ou sem formatação</param>
    /// <returns>CPF apenas com números</returns>
    public static string Normalize(string cpf)
    {
        return Regex.Replace(cpf, @"[^\d]", "");
    }
} 