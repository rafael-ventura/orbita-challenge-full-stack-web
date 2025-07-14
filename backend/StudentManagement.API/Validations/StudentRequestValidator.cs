using StudentManagement.Application.DTOs;
using System.Text.RegularExpressions;

namespace StudentManagement.API.Validations;

public static class StudentRequestValidator
{
    public static List<string> ValidateCreateStudent(CreateStudentDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 100)
            errors.Add("Name is required and must be at most 100 characters.");

        if (string.IsNullOrWhiteSpace(dto.Email) || dto.Email.Length > 100 || !IsValidEmail(dto.Email))
            errors.Add("Email is required, must be at most 100 characters and valid.");

        if (string.IsNullOrWhiteSpace(dto.RA) || dto.RA.Length > 20 || !IsValidRA(dto.RA))
            errors.Add("RA is required, must be at most 20 characters and valid.");

        if (string.IsNullOrWhiteSpace(dto.CPF) || dto.CPF.Length > 14 || !IsValidCPF(dto.CPF))
            errors.Add("CPF is required, must be at most 14 characters and valid.");

        return errors;
    }

    public static List<string> ValidateUpdateStudent(UpdateStudentDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 100)
            errors.Add("Name is required and must be at most 100 characters.");

        if (string.IsNullOrWhiteSpace(dto.Email) || dto.Email.Length > 100 || !IsValidEmail(dto.Email))
            errors.Add("Email is required, must be at most 100 characters and valid.");

        return errors;
    }

    // --- Funções utilitárias de validação e normalização ---
    public static bool IsValidCPF(string cpf)
    {
        cpf = Regex.Replace(cpf, @"[^\d]", "");
        if (cpf.Length != 11) return false;
        if (cpf.All(c => c == cpf[0])) return false;
        int sum = 0;
        for (int i = 0; i < 9; i++) sum += int.Parse(cpf[i].ToString()) * (10 - i);
        int remainder = sum % 11;
        int digit1 = remainder < 2 ? 0 : 11 - remainder;
        sum = 0;
        for (int i = 0; i < 10; i++) sum += int.Parse(cpf[i].ToString()) * (11 - i);
        remainder = sum % 11;
        int digit2 = remainder < 2 ? 0 : 11 - remainder;
        return int.Parse(cpf[9].ToString()) == digit1 && int.Parse(cpf[10].ToString()) == digit2;
    }

    public static bool IsValidRA(string ra)
    {
        ra = Regex.Replace(ra, @"[^\d]", "");
        return ra.Length >= 6 && ra.Length <= 20 && ra.All(char.IsDigit);
    }

    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static string NormalizeCPF(string cpf)
    {
        return Regex.Replace(cpf, @"[^\d]", "");
    }

    public static string NormalizeRA(string ra)
    {
        return Regex.Replace(ra, @"[^\d]", "");
    }

    public static string FormatCPF(string cpf)
    {
        var normalized = NormalizeCPF(cpf);
        if (normalized.Length != 11)
            return cpf;
        return $"{normalized.Substring(0, 3)}.{normalized.Substring(3, 3)}.{normalized.Substring(6, 3)}-{normalized.Substring(9, 2)}";
    }
} 