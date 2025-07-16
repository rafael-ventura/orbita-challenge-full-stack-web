using FluentAssertions;
using Moq;
using StudentManagement.API.Validations;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Xunit;
using StudentManagement.Application.Helpers;

namespace StudentManagement.Tests.Unit.Application;

public class StudentRequestValidatorTests
{
    private readonly Mock<ILogger<StudentRequestValidator>> _mockLogger;
    private readonly StudentRequestValidator _validator;

    public StudentRequestValidatorTests()
    {
        _mockLogger = new Mock<ILogger<StudentRequestValidator>>();
        _validator = new StudentRequestValidator();
    }

    [Fact]
    public async Task ValidateCreateStudentAsync_WithValidData_ShouldReturnEmptyErrors()
    {
        // Arrange
        var validDto = new CreateStudentDto
        {
            Name = "João Silva",
            Email = "joao@example.com",
            RA = "123456",
            CPF = "52998224725"
        };

        // Act
        var errors = await _validator.ValidateCreateStudentAsync(validDto);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public async Task ValidateCreateStudentAsync_WithInvalidCPF_ShouldReturnError()
    {
        // Arrange
        var invalidDto = new CreateStudentDto
        {
            Name = "João Silva",
            Email = "joao@example.com",
            RA = "123456",
            CPF = "12345678901" // CPF inválido
        };

        // Act
        var errors = await _validator.ValidateCreateStudentAsync(invalidDto);

        // Assert
        errors.Should().Contain("CPF inválido. Verifique se o número está correto.");
    }

    // Remover testes que dependem de validação externa e logger
    // [Fact]
    // public async Task ValidateCreateStudentAsync_WithValidCPFButExternalValidationFails_ShouldReturnError() { ... }
    // [Fact]
    // public async Task ValidateCreateStudentAsync_WithExternalValidationException_ShouldNotBlockCreation() { ... }

    [Fact]
    public async Task ValidateCreateStudentAsync_WithEmptyName_ShouldReturnError()
    {
        // Arrange
        var invalidDto = new CreateStudentDto
        {
            Name = "",
            Email = "joao@example.com",
            RA = "123456",
            CPF = "52998224725"
        };

        // Act
        var errors = await _validator.ValidateCreateStudentAsync(invalidDto);

        // Assert
        errors.Should().Contain("Name is required and must be at most 100 characters.");
    }

    [Fact]
    public async Task ValidateCreateStudentAsync_WithInvalidEmail_ShouldReturnError()
    {
        // Arrange
        var invalidDto = new CreateStudentDto
        {
            Name = "João Silva",
            Email = "invalid-email",
            RA = "123456",
            CPF = "52998224725"
        };

        // Act
        var errors = await _validator.ValidateCreateStudentAsync(invalidDto);

        // Assert
        errors.Should().Contain("Email is required, must be at most 100 characters and valid.");
    }

    [Fact]
    public async Task ValidateCreateStudentAsync_WithInvalidRA_ShouldReturnError()
    {
        // Arrange
        var invalidDto = new CreateStudentDto
        {
            Name = "João Silva",
            Email = "joao@example.com",
            RA = "123", // RA muito curto
            CPF = "52998224725"
        };

        // Act
        var errors = await _validator.ValidateCreateStudentAsync(invalidDto);

        // Assert
        errors.Should().Contain("RA is required, must be at most 20 characters and valid.");
    }

    [Fact]
    public void ValidateUpdateStudent_WithValidData_ShouldReturnEmptyErrors()
    {
        // Arrange
        var validDto = new UpdateStudentDto
        {
            Name = "João Silva",
            Email = "joao@example.com"
        };

        // Act
        var errors = StudentRequestValidator.ValidateUpdateStudent(validDto);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void ValidateUpdateStudent_WithInvalidData_ShouldReturnErrors()
    {
        // Arrange
        var invalidDto = new UpdateStudentDto
        {
            Name = "",
            Email = "invalid-email"
        };

        // Act
        var errors = StudentRequestValidator.ValidateUpdateStudent(invalidDto);

        // Assert
        errors.Should().Contain("Name is required and must be at most 100 characters.");
        errors.Should().Contain("Email is required, must be at most 100 characters and valid.");
    }

    [Theory]
    [InlineData("52998224725", true)]
    [InlineData("12345678901", false)]
    [InlineData("529.982.247-25", true)]
    [InlineData("11111111111", false)]
    public void IsValidCPF_WithVariousCPFs_ShouldReturnExpectedResult(string cpf, bool expected)
    {
        // Act
        var result = CpfValidator.IsValid(cpf);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123456", true)]
    [InlineData("12345", false)] // Muito curto
    [InlineData("123456789012345678901", false)] // Muito longo
    [InlineData("abc123", false)] // Contém letras
    public void IsValidRA_WithVariousRAs_ShouldReturnExpectedResult(string ra, bool expected)
    {
        // Act
        var result = StudentRequestValidator.IsValidRa(ra);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("invalid-email", false)]
    [InlineData("test@", false)]
    [InlineData("@example.com", false)]
    public void IsValidEmail_WithVariousEmails_ShouldReturnExpectedResult(string email, bool expected)
    {
        // Act
        var result = StudentRequestValidator.IsValidEmail(email);

        // Assert
        result.Should().Be(expected);
    }
} 