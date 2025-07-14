using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;
using StudentManagement.Infrastructure.Contexts;
using StudentManagement.Infrastructure.Repositories;
using Xunit;

namespace StudentManagement.Tests.Unit.Infrastructure;

public class StudentRepositoryUnitTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly StudentRepository _repository;

    public StudentRepositoryUnitTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new StudentRepository(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStudents()
    {
        // Arrange
        var students = new List<Student>
        {
            new() { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@example.com", RA = "12345", CPF = "12345678901" },
            new() { Id = Guid.NewGuid(), Name = "Jane Smith", Email = "jane@example.com", RA = "67890", CPF = "98765432109" }
        };

        await _context.Students.AddRangeAsync(students);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

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
            RA = "12345",
            CPF = "12345678901"
        };

        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(studentId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(studentId);
        result.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentStudent_ShouldReturnNull()
    {
        // Arrange
        var studentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(studentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateStudent()
    {
        // Arrange
        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            RA = "12345",
            CPF = "12345678901"
        };

        // Act
        var result = await _repository.CreateAsync(student);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(student.Id);
        result.Name.Should().Be("John Doe");

        var savedStudent = await _context.Students.FindAsync(student.Id);
        savedStudent.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateStudent()
    {
        // Arrange
        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            RA = "12345",
            CPF = "12345678901"
        };

        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        student.Name = "John Updated";

        // Act
        var result = await _repository.UpdateAsync(student);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Updated");

        var updatedStudent = await _context.Students.FindAsync(student.Id);
        updatedStudent!.Name.Should().Be("John Updated");
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteStudent()
    {
        // Arrange
        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            RA = "12345",
            CPF = "12345678901"
        };

        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(student.Id);

        // Assert
        var deletedStudent = await _context.Students.FindAsync(student.Id);
        deletedStudent.Should().BeNull();
    }

    [Fact]
    public async Task GetByRAAsync_WithExistingStudent_ShouldReturnStudent()
    {
        // Arrange
        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            RA = "12345",
            CPF = "12345678901"
        };

        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByRAAsync("12345");

        // Assert
        result.Should().NotBeNull();
        result!.RA.Should().Be("12345");
    }

    [Fact]
    public async Task GetByCPFAsync_WithExistingStudent_ShouldReturnStudent()
    {
        // Arrange
        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            RA = "12345",
            CPF = "12345678901"
        };

        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCPFAsync("12345678901");

        // Assert
        result.Should().NotBeNull();
        result!.CPF.Should().Be("12345678901");
    }

    [Fact]
    public async Task ExistsAsync_WithExistingStudent_ShouldReturnTrue()
    {
        // Arrange
        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            RA = "12345",
            CPF = "12345678901"
        };

        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(student.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistentStudent_ShouldReturnFalse()
    {
        // Arrange
        var studentId = Guid.NewGuid();

        // Act
        var result = await _repository.ExistsAsync(studentId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsByRAAsync_WithExistingStudent_ShouldReturnTrue()
    {
        // Arrange
        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            RA = "12345",
            CPF = "12345678901"
        };

        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsByRAAsync("12345");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByCPFAsync_WithExistingStudent_ShouldReturnTrue()
    {
        // Arrange
        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            RA = "12345",
            CPF = "12345678901"
        };

        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsByCPFAsync("12345678901");

        // Assert
        result.Should().BeTrue();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
} 