using System.ComponentModel.DataAnnotations;
using StudentManagement.Application.Validations;

namespace StudentManagement.Application.DTOs;

public class StudentDto
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email deve ser válido")]
    [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "RA é obrigatório")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "RA deve ter entre 6 e 20 caracteres")]
    [RegularExpression(@"^\d+$", ErrorMessage = "RA deve conter apenas números")]
    public string RA { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "CPF deve ter entre 11 e 14 caracteres")]
    [RegularExpression(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF deve estar no formato 12345678901 ou 123.456.789-01")]
    public string CPF { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateStudentDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email deve ser válido")]
    [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "RA é obrigatório")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "RA deve ter entre 6 e 20 caracteres")]
    [RegularExpression(@"^\d+$", ErrorMessage = "RA deve conter apenas números")]
    public string RA { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "CPF deve ter entre 11 e 14 caracteres")]
    [RegularExpression(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF deve estar no formato 12345678901 ou 123.456.789-01")]
    [CustomValidation(typeof(CreateStudentDto), nameof(ValidateCPF))]
    public string CPF { get; set; } = string.Empty;
    
    public static ValidationResult? ValidateCPF(string cpf, ValidationContext context)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return new ValidationResult("CPF is required");
        
        if (!StudentDataHelper.IsValidCPF(cpf))
            return new ValidationResult("Invalid CPF");
        
        return ValidationResult.Success;
    }
}

public class UpdateStudentDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email deve ser válido")]
    [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
    public string Email { get; set; } = string.Empty;
} 