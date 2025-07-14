using FluentAssertions;
using StudentManagement.Application.Validations;
using Xunit;

namespace StudentManagement.Tests.Unit.Application;

public class StudentValidationTests
{
    [Fact]
    public void IsValidCPF_WithValidCPF_ShouldReturnTrue()
    {
        // Arrange
        const string validCpf = "52998224725";

        // Act
        var result = StudentDataHelper.IsValidCPF(validCpf);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidCPF_WithInvalidCPF_ShouldReturnFalse()
    {
        // Arrange
        const string invalidCpf = "12345678901";

        // Act
        var result = StudentDataHelper.IsValidCPF(invalidCpf);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidCPF_WithFormattedCPF_ShouldReturnTrue()
    {
        // Arrange
        const string formattedCpf = "529.982.247-25";

        // Act
        var result = StudentDataHelper.IsValidCPF(formattedCpf);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidRA_WithValidRA_ShouldReturnTrue()
    {
        // Arrange
        const string validRa = "123456";

        // Act
        var result = StudentDataHelper.IsValidRA(validRa);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidRA_WithShortRA_ShouldReturnFalse()
    {
        // Arrange
        const string shortRa = "12345";

        // Act
        var result = StudentDataHelper.IsValidRA(shortRa);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidEmail_WithValidEmail_ShouldReturnTrue()
    {
        // Arrange
        const string validEmail = "test@example.com";

        // Act
        var result = StudentDataHelper.IsValidEmail(validEmail);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidEmail_WithInvalidEmail_ShouldReturnFalse()
    {
        // Arrange
        const string invalidEmail = "invalid-email";

        // Act
        var result = StudentDataHelper.IsValidEmail(invalidEmail);

        // Assert
        result.Should().BeFalse();
    }
} 