using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Exceptions;
using StudentManagement.API.Validations;
using StudentManagement.Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using static StudentManagement.Domain.Exceptions.ErrorMessage;
using static StudentManagement.Domain.Exceptions.ErrorMessageExtensions;

namespace StudentManagement.API.Controllers.Student;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentController(
    IStudentService studentService, 
    StudentRequestValidator studentRequestValidator,
    ILogger<StudentController> logger) : ControllerBase
{
    /// <summary>
    /// Get all students
    /// </summary>
    /// <returns>List of all students</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StudentDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll()
    {
        var students = await studentService.GetAllAsync();
        return Ok(students);
    }

    /// <summary>
    /// Get student by ID
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <returns>Student information</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(StudentDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<StudentDto>> GetById(Guid id)
    {
        var student = await studentService.GetByIdAsync(id);
        return student != null ? Ok(student) : NotFound(new { message = string.Format(ErrorMessage.StudentNotFound.GetDescription(), id) });
    }

    /// <summary>
    /// Create a new student
    /// </summary>
    /// <param name="createStudentDto">Student data</param>
    /// <returns>Created student</returns>
    [HttpPost]
    [ProducesResponseType(typeof(StudentDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<StudentDto>> Create([FromBody] CreateStudentDto createStudentDto)
    {
        var errors = await studentRequestValidator.ValidateCreateStudentAsync(createStudentDto);
        if (errors.Any())
            return BadRequest(new { message = ErrorMessage.ValidationFailed.GetDescription(), errors });

        try
        {
            var createdStudent = await studentService.CreateAsync(createStudentDto);
            return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
        }
        catch (InvalidStudentDataException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update student information
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="updateStudentDto">Updated student data</param>
    /// <returns>Updated student</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(StudentDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<StudentDto>> Update(Guid id, [FromBody] UpdateStudentDto updateStudentDto)
    {
        var errors = StudentRequestValidator.ValidateUpdateStudent(updateStudentDto);
        if (errors.Any())
            return BadRequest(new { message = ErrorMessage.ValidationFailed.GetDescription(), errors });

        var updatedStudent = await studentService.UpdateAsync(id, updateStudentDto);
        return Ok(updatedStudent);
    }

    /// <summary>
    /// Delete a student
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Delete(Guid id)
    {
        await studentService.DeleteAsync(id);
        return NoContent();
    }
} 