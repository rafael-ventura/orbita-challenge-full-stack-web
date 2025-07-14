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
        var validCPF = "52998224725";

        // Act
        var result = StudentDataHelper.IsValidCPF(validCPF);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidCPF_WithInvalidCPF_ShouldReturnFalse()
    {
        // Arrange
        var invalidCPF = "12345678901";

        // Act
        var result = StudentDataHelper.IsValidCPF(invalidCPF);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidCPF_WithFormattedCPF_ShouldReturnTrue()
    {
        // Arrange
        var formattedCPF = "529.982.247-25";

        // Act
        var result = StudentDataHelper.IsValidCPF(formattedCPF);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidRA_WithValidRA_ShouldReturnTrue()
    {
        // Arrange
        var validRA = "123456";

        // Act
        var result = StudentDataHelper.IsValidRA(validRA);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidRA_WithShortRA_ShouldReturnFalse()
    {
        // Arrange
        var shortRA = "12345";

        // Act
        var result = StudentDataHelper.IsValidRA(shortRA);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidEmail_WithValidEmail_ShouldReturnTrue()
    {
        // Arrange
        var validEmail = "test@example.com";

        // Act
        var result = StudentDataHelper.IsValidEmail(validEmail);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidEmail_WithInvalidEmail_ShouldReturnFalse()
    {
        // Arrange
        var invalidEmail = "invalid-email";

        // Act
        var result = StudentDataHelper.IsValidEmail(invalidEmail);

        // Assert
        result.Should().BeFalse();
    }
} 