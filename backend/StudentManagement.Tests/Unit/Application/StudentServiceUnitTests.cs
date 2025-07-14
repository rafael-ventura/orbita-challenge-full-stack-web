using FluentAssertions;
using Moq;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Services;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Exceptions;
using StudentManagement.Domain.Interfaces.Repositories;
using Xunit;

namespace StudentManagement.Tests.Unit.Application;

public class StudentServiceUnitTests
{
    private readonly Mock<IStudentRepository> _mockRepository;
    private readonly StudentService _service;

    public StudentServiceUnitTests()
    {
        _mockRepository = new Mock<IStudentRepository>();
        _service = new StudentService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStudents()
    {
        // Arrange
        var students = new List<Student>
        {
            new() { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@example.com", RA = "123456", CPF = "11144477735" },
            new() { Id = Guid.NewGuid(), Name = "Jane Smith", Email = "jane@example.com", RA = "654321", CPF = "98765432100" }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(students);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(s => s.Name == "John Doe");
        result.Should().Contain(s => s.Name == "Jane Smith");
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingStudent_ShouldReturnStudent()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var student = new Student
        {
            Id = studentId,
            Name = "John Doe",
            Email = "john@example.com",
            RA = "123456",
            CPF = "11144477735"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(student);

        // Act
        var result = await _service.GetByIdAsync(studentId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("John Doe");
        result.Email.Should().Be("john@example.com");
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentStudent_ShouldReturnNull()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync((Student?)null);

        // Act
        var result = await _service.GetByIdAsync(studentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateStudent()
    {
        // Arrange
        var createDto = new CreateStudentDto
        {
            Name = "John Doe",
            Email = "john@example.com",
            RA = "123456",
            CPF = "11144477735"
        };

        var createdStudent = new Student
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            RA = "123456",
            CPF = "11144477735",
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.ExistsByRAAsync(It.IsAny<string>())).ReturnsAsync(false);
        _mockRepository.Setup(r => r.ExistsByCPFAsync(It.IsAny<string>())).ReturnsAsync(false);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Student>());
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Student>())).ReturnsAsync(createdStudent);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("john@example.com");
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Student>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithExistingRA_ShouldThrowException()
    {
        // Arrange
        var createDto = new CreateStudentDto
        {
            Name = "John Doe",
            Email = "john@example.com",
            RA = "123456",
            CPF = "11144477735"
        };

        _mockRepository.Setup(r => r.ExistsByRAAsync("123456")).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidStudentDataException>(() => 
            _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task CreateAsync_WithExistingCPF_ShouldThrowException()
    {
        // Arrange
        var createDto = new CreateStudentDto
        {
            Name = "John Doe",
            Email = "john@example.com",
            RA = "123456",
            CPF = "11144477735"
        };

        _mockRepository.Setup(r => r.ExistsByRAAsync(It.IsAny<string>())).ReturnsAsync(false);
        _mockRepository.Setup(r => r.ExistsByCPFAsync("11144477735")).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidStudentDataException>(() => 
            _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task CreateAsync_WithExistingEmail_ShouldThrowException()
    {
        // Arrange
        var createDto = new CreateStudentDto
        {
            Name = "John Doe",
            Email = "john@example.com",
            RA = "123456",
            CPF = "11144477735"
        };

        var existingStudents = new List<Student>
        {
            new() { Id = Guid.NewGuid(), Email = "john@example.com" }
        };

        _mockRepository.Setup(r => r.ExistsByRAAsync(It.IsAny<string>())).ReturnsAsync(false);
        _mockRepository.Setup(r => r.ExistsByCPFAsync(It.IsAny<string>())).ReturnsAsync(false);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(existingStudents);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidStudentDataException>(() => 
            _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateStudent()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var existingStudent = new Student
        {
            Id = studentId,
            Name = "John Doe",
            Email = "john@example.com",
            RA = "123456",
            CPF = "11144477735",
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var updateDto = new UpdateStudentDto
        {
            Name = "John Updated",
            Email = "john.updated@example.com"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(existingStudent);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Student>());
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Student>())).ReturnsAsync(existingStudent);

        // Act
        var result = await _service.UpdateAsync(studentId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Updated");
        result.Email.Should().Be("john.updated@example.com");
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Student>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentStudent_ShouldThrowException()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var updateDto = new UpdateStudentDto
        {
            Name = "John Updated",
            Email = "john.updated@example.com"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<StudentNotFoundException>(() => 
            _service.UpdateAsync(studentId, updateDto));
    }

    [Fact]
    public async Task UpdateAsync_WithExistingEmail_ShouldThrowException()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var existingStudent = new Student
        {
            Id = studentId,
            Name = "John Doe",
            Email = "john@example.com",
            RA = "123456",
            CPF = "11144477735"
        };

        var updateDto = new UpdateStudentDto
        {
            Name = "John Updated",
            Email = "jane@example.com" // Email já existente
        };

        var existingStudents = new List<Student>
        {
            new() { Id = Guid.NewGuid(), Email = "jane@example.com" }
        };

        _mockRepository.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(existingStudent);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(existingStudents);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidStudentDataException>(() => 
            _service.UpdateAsync(studentId, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_WithExistingStudent_ShouldDeleteStudent()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var existingStudent = new Student
        {
            Id = studentId,
            Name = "John Doe",
            Email = "john@example.com",
            RA = "123456",
            CPF = "11144477735"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(existingStudent);

        // Act
        await _service.DeleteAsync(studentId);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(studentId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentStudent_ShouldThrowException()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<StudentNotFoundException>(() => 
            _service.DeleteAsync(studentId));
    }
} 