using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Domain.Entities;

public class Student
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string RA { get; set; } = string.Empty;
    
    [Required]
    [StringLength(14)]
    public string CPF { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 