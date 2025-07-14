using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Services;
using StudentManagement.Application.Helpers;
using StudentManagement.Domain.Interfaces.Repositories;
using StudentManagement.Infrastructure.Contexts;
using StudentManagement.Infrastructure.Repositories;
using StudentManagement.API.Controllers.Student;
using StudentManagement.API.Validations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace StudentManagement.Tests.Integration;

public class StudentControllerIntegrationTests : IDisposable
{
    private readonly StudentController _controller;
    private readonly ApplicationDbContext _context;
    private readonly Mock<ILogger<StudentController>> _mockLogger;
    private readonly Mock<IExternalCpfValidator> _mockExternalCpfValidator;
    private readonly Mock<ILogger<StudentRequestValidator>> _mockValidatorLogger;
    private readonly StudentRequestValidator _studentRequestValidator;

    public StudentControllerIntegrationTests()
    {
        var services = new ServiceCollection();
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid()));
        
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStudentService, StudentService>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        _context.Database.EnsureCreated();
        
        var studentService = serviceProvider.GetRequiredService<IStudentService>();
        
        _mockLogger = new Mock<ILogger<StudentController>>();
        _mockExternalCpfValidator = new Mock<IExternalCpfValidator>();
        _mockValidatorLogger = new Mock<ILogger<StudentRequestValidator>>();
        _studentRequestValidator = new StudentRequestValidator(_mockExternalCpfValidator.Object, _mockValidatorLogger.Object);
        
        _controller = new StudentController(
            studentService, 
            _studentRequestValidator,
            _mockLogger.Object);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    [Fact]
    public async Task CreateStudent_WithValidData_ReturnsCreated()
    {
        // Arrange
        var createStudentDto = new CreateStudentDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            RA = "123456789",
            CPF = "52998224725" // CPF válido
        };

        _mockExternalCpfValidator
            .Setup(x => x.IsCpfValidAsync(createStudentDto.CPF))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Create(createStudentDto);
        
        // Assert
        Assert.IsType<ActionResult<StudentDto>>(result);
        Assert.NotNull(result.Result);
        
        if (result.Result is CreatedAtActionResult createdResult)
        {
            var createdStudent = Assert.IsType<StudentDto>(createdResult.Value);
            Assert.Equal(createStudentDto.Name, createdStudent.Name);
            Assert.Equal(createStudentDto.Email, createdStudent.Email);
            Assert.Equal(createStudentDto.RA, createdStudent.RA);
        }
        else
        {
            Assert.Fail("Expected CreatedAtActionResult but got " + result.Result.GetType().Name);
        }
    }

    [Fact]
    public async Task GetStudent_WithValidId_ReturnsStudent()
    {
        // Arrange
        var createStudentDto = new CreateStudentDto
        {
            Name = "Jane Smith",
            Email = "jane.smith@example.com",
            RA = "987654321",
            CPF = "52998224725" // CPF válido
        };

        _mockExternalCpfValidator
            .Setup(x => x.IsCpfValidAsync(createStudentDto.CPF))
            .ReturnsAsync(true);

        var createResult = await _controller.Create(createStudentDto);
        
        StudentDto? createdStudent = null;
        if (createResult.Result is CreatedAtActionResult createdActionResult)
        {
            createdStudent = Assert.IsType<StudentDto>(createdActionResult.Value);
        }
        else
        {
            Assert.Fail("Expected CreatedAtActionResult");
        }

        // Act
        var getResult = await _controller.GetById(createdStudent!.Id);
        
        // Assert
        Assert.IsType<ActionResult<StudentDto>>(getResult);
        Assert.NotNull(getResult.Result);
        
        if (getResult.Result is OkObjectResult okResult)
        {
            var retrievedStudent = Assert.IsType<StudentDto>(okResult.Value);
            Assert.Equal(createdStudent.Id, retrievedStudent.Id);
            Assert.Equal(createdStudent.Name, retrievedStudent.Name);
        }
        else
        {
            Assert.Fail("Expected OkObjectResult but got " + getResult.Result.GetType().Name);
        }
    }

    [Fact]
    public async Task GetAllStudents_ReturnsStudentsList()
    {
        // Arrange
        var createStudentDto1 = new CreateStudentDto
        {
            Name = "Alice Johnson",
            Email = "alice.johnson@example.com",
            RA = "111111111",
            CPF = "52998224725" // CPF válido
        };

        var createStudentDto2 = new CreateStudentDto
        {
            Name = "Bob Wilson",
            Email = "bob.wilson@example.com",
            RA = "222222222",
            CPF = "12345678901" // CPF válido diferente
        };

        _mockExternalCpfValidator
            .Setup(x => x.IsCpfValidAsync(createStudentDto1.CPF))
            .ReturnsAsync(true);

        _mockExternalCpfValidator
            .Setup(x => x.IsCpfValidAsync(createStudentDto2.CPF))
            .ReturnsAsync(true);

        // Act
        await _controller.Create(createStudentDto1);
        await _controller.Create(createStudentDto2);

        var result = await _controller.GetAll();
        
        // Assert
        Assert.IsType<ActionResult<IEnumerable<StudentDto>>>(result);
        Assert.NotNull(result.Result);
        
        if (result.Result is OkObjectResult okResult)
        {
            var students = okResult.Value as IEnumerable<StudentDto>;
            Assert.NotNull(students);
            Assert.True(students!.Count() >= 2);
        }
        else
        {
            Assert.Fail("Expected OkObjectResult but got " + result.Result.GetType().Name);
        }
    }

    [Fact]
    public async Task UpdateStudent_WithValidData_ReturnsUpdatedStudent()
    {
        // Arrange
        var createStudentDto = new CreateStudentDto
        {
            Name = "Original Name",
            Email = "original@example.com",
            RA = "333333333",
            CPF = "52998224725" // CPF válido
        };

        _mockExternalCpfValidator
            .Setup(x => x.IsCpfValidAsync(createStudentDto.CPF))
            .ReturnsAsync(true);

        var createResult = await _controller.Create(createStudentDto);
        
        StudentDto? createdStudent = null;
        if (createResult.Result is CreatedAtActionResult createdActionResult)
        {
            createdStudent = Assert.IsType<StudentDto>(createdActionResult.Value);
        }
        else
        {
            Assert.Fail("Expected CreatedAtActionResult");
        }

        var updateStudentDto = new UpdateStudentDto
        {
            Name = "Updated Name",
            Email = "updated@example.com"
        };

        // Act
        var updateResult = await _controller.Update(createdStudent!.Id, updateStudentDto);
        
        // Assert
        Assert.IsType<ActionResult<StudentDto>>(updateResult);
        Assert.NotNull(updateResult.Result);
        
        if (updateResult.Result is OkObjectResult okResult)
        {
            var updatedStudent = Assert.IsType<StudentDto>(okResult.Value);
            Assert.Equal(updateStudentDto.Name, updatedStudent.Name);
            Assert.Equal(updateStudentDto.Email, updatedStudent.Email);
        }
        else
        {
            Assert.Fail("Expected OkObjectResult but got " + updateResult.Result.GetType().Name);
        }
    }

    [Fact]
    public async Task DeleteStudent_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var createStudentDto = new CreateStudentDto
        {
            Name = "Student to Delete",
            Email = "delete@example.com",
            RA = "444444444",
            CPF = "52998224725" // CPF válido
        };

        _mockExternalCpfValidator
            .Setup(x => x.IsCpfValidAsync(createStudentDto.CPF))
            .ReturnsAsync(true);

        var createResult = await _controller.Create(createStudentDto);
        
        StudentDto? createdStudent = null;
        if (createResult.Result is CreatedAtActionResult createdActionResult)
        {
            createdStudent = Assert.IsType<StudentDto>(createdActionResult.Value);
        }
        else
        {
            Assert.Fail("Expected CreatedAtActionResult");
        }

        // Act
        var deleteResult = await _controller.Delete(createdStudent!.Id);
        
        // Assert
        Assert.IsType<NoContentResult>(deleteResult);
    }
} 