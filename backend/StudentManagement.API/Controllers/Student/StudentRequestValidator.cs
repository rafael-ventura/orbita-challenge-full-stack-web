using StudentManagement.Application.DTOs;
using StudentManagement.Application.Helpers;
using System.Text.RegularExpressions;

namespace StudentManagement.API.Validations;

public class StudentRequestValidator(ILogger<StudentRequestValidator> logger)
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
        else if (!CpfValidator.IsValid(dto.CPF))
        {
            errors.Add("CPF inválido. Verifique se o número está correto.");
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

    public static string NormalizeRa(string ra)
    {
        return Regex.Replace(ra, @"[^\d]", "");
    }
} 