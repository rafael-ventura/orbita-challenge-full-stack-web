using StudentManagement.Application.DTOs;
using StudentManagement.Application.Helpers;
using System.Text.RegularExpressions;
using StudentManagement.Domain.Exceptions;

namespace StudentManagement.API.Validations;

public class StudentRequestValidator
{
    public async Task<List<string>> ValidateCreateStudentAsync(CreateStudentDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 100)
            errors.Add(ErrorMessage.NameRequired.GetDescription());

        if (string.IsNullOrWhiteSpace(dto.Email) || dto.Email.Length > 100 || !IsValidEmail(dto.Email))
            errors.Add(ErrorMessage.EmailRequired.GetDescription());

        if (string.IsNullOrWhiteSpace(dto.RA) || dto.RA.Length > 20 || !IsValidRa(dto.RA))
            errors.Add(ErrorMessage.RaRequired.GetDescription());

        if (string.IsNullOrWhiteSpace(dto.CPF) || dto.CPF.Length > 14)
            errors.Add(ErrorMessage.CpfRequired.GetDescription());
        else if (!CpfValidator.IsValid(dto.CPF))
        {
            errors.Add(ErrorMessage.CpfInvalid.GetDescription());
        }

        return errors;
    }

    public static List<string> ValidateUpdateStudent(UpdateStudentDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 100)
            errors.Add(ErrorMessage.NameRequired.GetDescription());

        if (string.IsNullOrWhiteSpace(dto.Email) || dto.Email.Length > 100 || !IsValidEmail(dto.Email))
            errors.Add(ErrorMessage.EmailRequired.GetDescription());

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