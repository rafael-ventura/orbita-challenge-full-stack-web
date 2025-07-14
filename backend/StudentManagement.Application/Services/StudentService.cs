using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Exceptions;
using StudentManagement.Domain.Interfaces.Repositories;
using StudentManagement.Application.Validations;

namespace StudentManagement.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        return students.Select(MapToDto);
    }

    public async Task<StudentDto?> GetByIdAsync(Guid id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        return student != null ? MapToDto(student) : null;
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto createStudentDto)
    {
        await ValidateStudentDataForCreation(createStudentDto);

        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = createStudentDto.Name.Trim(),
            Email = createStudentDto.Email.Trim().ToLower(),
            RA = StudentDataHelper.NormalizeRA(createStudentDto.RA),
            CPF = StudentDataHelper.NormalizeCPF(createStudentDto.CPF),
            CreatedAt = DateTime.UtcNow
        };

        var createdStudent = await _studentRepository.CreateAsync(student);
        return MapToDto(createdStudent);
    }

    public async Task<StudentDto> UpdateAsync(Guid id, UpdateStudentDto updateStudentDto)
    {
        var existingStudent = await _studentRepository.GetByIdAsync(id);
        if (existingStudent == null)
        {
            throw new StudentNotFoundException(id);
        }

        // Business validations for update
        await ValidateStudentDataForUpdate(id, updateStudentDto);

        existingStudent.Name = updateStudentDto.Name.Trim();
        existingStudent.Email = updateStudentDto.Email.Trim().ToLower();
        existingStudent.UpdatedAt = DateTime.UtcNow;

        var updatedStudent = await _studentRepository.UpdateAsync(existingStudent);
        return MapToDto(updatedStudent);
    }

    public async Task DeleteAsync(Guid id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
        {
            throw new StudentNotFoundException(id);
        }

        await _studentRepository.DeleteAsync(id);
    }

    private async Task ValidateStudentDataForCreation(CreateStudentDto createStudentDto)
    {
        var errors = new List<string>();

        // Custom validations
        if (string.IsNullOrWhiteSpace(createStudentDto.Name))
        {
            errors.Add("Name cannot be empty");
        }

        if (!StudentDataHelper.IsValidEmail(createStudentDto.Email))
        {
            errors.Add("Email must be valid");
        }

        if (!StudentDataHelper.IsValidRA(createStudentDto.RA))
        {
            errors.Add("RA must contain only numbers and have at least 6 digits");
        }

        if (!StudentDataHelper.IsValidCPF(createStudentDto.CPF))
        {
            errors.Add("CPF must be valid");
        }

        // Uniqueness validations
        if (await _studentRepository.ExistsByRAAsync(StudentDataHelper.NormalizeRA(createStudentDto.RA)))
        {
            errors.Add($"RA '{createStudentDto.RA}' already exists");
        }

        if (await _studentRepository.ExistsByCPFAsync(StudentDataHelper.NormalizeCPF(createStudentDto.CPF)))
        {
            errors.Add($"CPF '{createStudentDto.CPF}' already exists");
        }

        var existingStudentByEmail = await _studentRepository.GetAllAsync();
        if (existingStudentByEmail.Any(s => s.Email.Equals(createStudentDto.Email.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            errors.Add($"Email '{createStudentDto.Email}' already exists");
        }

        if (errors.Any())
        {
            throw new InvalidStudentDataException(string.Join("; ", errors));
        }
    }

    private async Task ValidateStudentDataForUpdate(Guid id, UpdateStudentDto updateStudentDto)
    {
        var errors = new List<string>();

        // Custom validations
        if (string.IsNullOrWhiteSpace(updateStudentDto.Name))
        {
            errors.Add("Name cannot be empty");
        }

        if (!StudentDataHelper.IsValidEmail(updateStudentDto.Email))
        {
            errors.Add("Email must be valid");
        }

        // Unique email validation (excluding the current student)
        var existingStudentByEmail = await _studentRepository.GetAllAsync();
        if (existingStudentByEmail.Any(s => s.Id != id && s.Email.Equals(updateStudentDto.Email.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            errors.Add($"Email '{updateStudentDto.Email}' already exists");
        }

        if (errors.Any())
        {
            throw new InvalidStudentDataException(string.Join("; ", errors));
        }
    }

    private static StudentDto MapToDto(Student student)
    {
        return new StudentDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            RA = student.RA,
            CPF = StudentDataHelper.FormatCPF(student.CPF), // Formats CPF for display
            CreatedAt = student.CreatedAt,
            UpdatedAt = student.UpdatedAt
        };
    }
} 