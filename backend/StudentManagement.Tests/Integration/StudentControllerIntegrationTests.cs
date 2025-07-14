using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Services;
using StudentManagement.Application.Helpers;
using StudentManagement.Domain.Interfaces.Repositories;
using StudentManagement.Infrastructure.Contexts;
using StudentManagement.Infrastructure.Repositories;
using StudentManagement.API.Controllers.Student;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace StudentManagement.Tests.Integration;

public class StudentControllerIntegrationTests
{
    private readonly StudentController _controller;
    private readonly ApplicationDbContext _context;

    public StudentControllerIntegrationTests()
    {
        var services = new ServiceCollection();
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid()));
        
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStudentService, StudentService>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var studentService = serviceProvider.GetRequiredService<IStudentService>();
        
        _controller = new StudentController(
            studentService, 
            null, // ExternalCpfValidator mock
            null); // ILogger mock
    }

    [Fact]
    public async Task CreateStudent_WithValidData_ReturnsCreated()
    {
        var createStudentDto = new CreateStudentDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            RA = "123456789",
            CPF = "12345678901"
        };

        var result = await _controller.Create(createStudentDto);
        
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdStudent = Assert.IsType<StudentDto>(createdResult.Value);
        
        Assert.Equal(createStudentDto.Name, createdStudent.Name);
        Assert.Equal(createStudentDto.Email, createdStudent.Email);
        Assert.Equal(createStudentDto.RA, createdStudent.RA);
    }

    [Fact]
    public async Task GetStudent_WithValidId_ReturnsStudent()
    {
        var createStudentDto = new CreateStudentDto
        {
            Name = "Jane Smith",
            Email = "jane.smith@example.com",
            RA = "987654321",
            CPF = "98765432109"
        };

        var createResult = await _controller.Create(createStudentDto);
        var createdStudent = Assert.IsType<StudentDto>(((CreatedAtActionResult)createResult.Result).Value);

        var getResult = await _controller.GetById(createdStudent.Id);
        
        var okResult = Assert.IsType<OkObjectResult>(getResult.Result);
        var retrievedStudent = Assert.IsType<StudentDto>(okResult.Value);
        
        Assert.Equal(createdStudent.Id, retrievedStudent.Id);
        Assert.Equal(createdStudent.Name, retrievedStudent.Name);
    }

    [Fact]
    public async Task GetAllStudents_ReturnsStudentsList()
    {
        var createStudentDto1 = new CreateStudentDto
        {
            Name = "Alice Johnson",
            Email = "alice.johnson@example.com",
            RA = "111111111",
            CPF = "11111111111"
        };

        var createStudentDto2 = new CreateStudentDto
        {
            Name = "Bob Wilson",
            Email = "bob.wilson@example.com",
            RA = "222222222",
            CPF = "22222222222"
        };

        await _controller.Create(createStudentDto1);
        await _controller.Create(createStudentDto2);

        var result = await _controller.GetAll();
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var students = Assert.IsType<IEnumerable<StudentDto>>(okResult.Value);
        
        Assert.True(students.Count() >= 2);
    }

    [Fact]
    public async Task UpdateStudent_WithValidData_ReturnsUpdatedStudent()
    {
        var createStudentDto = new CreateStudentDto
        {
            Name = "Original Name",
            Email = "original@example.com",
            RA = "333333333",
            CPF = "33333333333"
        };

        var createResult = await _controller.Create(createStudentDto);
        var createdStudent = Assert.IsType<StudentDto>(((CreatedAtActionResult)createResult.Result).Value);

        var updateStudentDto = new UpdateStudentDto
        {
            Name = "Updated Name",
            Email = "updated@example.com"
        };

        var updateResult = await _controller.Update(createdStudent.Id, updateStudentDto);
        
        var okResult = Assert.IsType<OkObjectResult>(updateResult.Result);
        var updatedStudent = Assert.IsType<StudentDto>(okResult.Value);
        
        Assert.Equal(updateStudentDto.Name, updatedStudent.Name);
        Assert.Equal(updateStudentDto.Email, updatedStudent.Email);
        Assert.Equal(createdStudent.RA, updatedStudent.RA);
        Assert.Equal(createdStudent.CPF, updatedStudent.CPF);
    }

    [Fact]
    public async Task DeleteStudent_WithValidId_ReturnsNoContent()
    {
        var createStudentDto = new CreateStudentDto
        {
            Name = "To Delete",
            Email = "todelete@example.com",
            RA = "444444444",
            CPF = "44444444444"
        };

        var createResult = await _controller.Create(createStudentDto);
        var createdStudent = Assert.IsType<StudentDto>(((CreatedAtActionResult)createResult.Result).Value);

        var deleteResult = await _controller.Delete(createdStudent.Id);
        
        Assert.IsType<NoContentResult>(deleteResult);
    }
} 