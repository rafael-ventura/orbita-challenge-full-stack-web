using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using System.Text.RegularExpressions;

namespace StudentManagement.API.Validations;

public class StudentRequestValidator(
    IExternalCpfValidator externalCpfValidator,
    ILogger<StudentRequestValidator> logger)
{
    public async Task<List<string>> ValidateCreateStudentAsync(CreateStudentDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 100)
            errors.Add("Name is required and must be at most 100 characters.");

        if (string.IsNullOrWhiteSpace(dto.Email) || dto.Email.Length > 100 || !IsValidEmail(dto.Email))
            errors.Add("Email is required, must be at most 100 characters and valid.");

        if (string.IsNullOrWhiteSpace(dto.RA) || dto.RA.Length > 20 || !IsValidRa(dto.RA))
            errors.Add("RA is required, must be at most 20 characters and valid.");

        if (string.IsNullOrWhiteSpace(dto.CPF) || dto.CPF.Length > 14)
            errors.Add("CPF is required, must be at most 14 characters");
        else
        {
            var externalValidationResult = await ValidateExternalCpf(dto.CPF);
            if (externalValidationResult.HasError)
            {
                errors.Add(externalValidationResult.ErrorMessage);
            }
        }

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

    private async Task<(bool HasError, string ErrorMessage)> ValidateExternalCpf(string cpf)
    {
        try
        {
            var isValid = await externalCpfValidator.IsCpfValidAsync(cpf);
            return !isValid ? (true, "CPF não encontrado na base externa (Speedio API).") : (false, string.Empty);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "External CPF validation service unavailable for CPF: {CPF}", cpf);
            return (false, string.Empty);
        }
    }

    public static bool IsValidCpf(string cpf)
    {
        cpf = Regex.Replace(cpf, @"[^\d]", "");
        if (cpf.Length != 11) return false;
        if (cpf.All(c => c == cpf[0])) return false;
        var sum = 0;
        for (var i = 0; i < 9; i++) sum += int.Parse(cpf[i].ToString()) * (10 - i);
        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;
        sum = 0;
        for (var i = 0; i < 10; i++) sum += int.Parse(cpf[i].ToString()) * (11 - i);
        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;
        return int.Parse(cpf[9].ToString()) == digit1 && int.Parse(cpf[10].ToString()) == digit2;
    }

    public static bool IsValidRa(string ra)
    {
        ra = Regex.Replace(ra, @"[^\d]", "");
        return ra.Length is >= 6 and <= 20 && ra.All(char.IsDigit);
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

    private static string NormalizeCpf(string cpf)
    {
        return Regex.Replace(cpf, @"[^\d]", "");
    }

    public static string NormalizeRa(string ra)
    {
        return Regex.Replace(ra, @"[^\d]", "");
    }

    public static string FormatCpf(string cpf)
    {
        var normalized = NormalizeCpf(cpf);
        return normalized.Length != 11 ? cpf : $"{normalized[..3]}.{normalized.Substring(3, 3)}.{normalized.Substring(6, 3)}-{normalized.Substring(9, 2)}";
    }
} 