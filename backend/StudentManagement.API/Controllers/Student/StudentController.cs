using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Exceptions;
using StudentManagement.API.Validations;
using StudentManagement.Application.Helpers;

namespace StudentManagement.API.Controllers.Student;

[ApiController]
[Route("api/[controller]")]
public class StudentController(
    IStudentService studentService, 
    ExternalCpfValidator externalCpfValidator,
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
        try
        {
            var students = await studentService.GetAllAsync();
            return Ok(students);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all students");
            return StatusCode(500, new { message = "Internal server error" });
        }
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
        try
        {
            var student = await studentService.GetByIdAsync(id);
            return student != null ? Ok(student) : NotFound(new { message = $"Student with ID {id} not found" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting student with ID {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create a new student
    /// </summary>
    /// <param name="createStudentDto">Student data</param>
    /// <returns>Created student</returns>
    [HttpPost]
    [ProducesResponseType(typeof(StudentDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<StudentDto>> Create([FromBody] CreateStudentDto createStudentDto)
    {
        // VALIDAÇÃO DE DADOS DE ENTRADA - Camada API
        // 1. Validação centralizada de todos os campos obrigatórios
        var errors = StudentRequestValidator.ValidateCreateStudent(createStudentDto);
        if (errors.Any())
            return BadRequest(new { message = "Validation failed", errors });

        // 2. Validação customizada de CPF na API (demonstração de integração HTTP)
        try
        {
            if (!await externalCpfValidator.IsCpfValidAsync(createStudentDto.CPF))
            {
                return BadRequest(new { 
                    message = "CPF validation failed", 
                    error = "CPF não encontrado na base externa (Speedio API)" 
                });
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "External CPF validation service unavailable");
            // Em caso de falha na API externa, continua com a validação local
        }

        try
        {
            var createdStudent = await studentService.CreateAsync(createStudentDto);
            return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
        }
        catch (InvalidStudentDataException ex)
        {
            logger.LogWarning(ex, "Invalid student data provided");
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating student");
            return StatusCode(500, new { message = "Internal server error" });
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
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<StudentDto>> Update(Guid id, [FromBody] UpdateStudentDto updateStudentDto)
    {
        // VALIDAÇÃO DE DADOS DE ENTRADA - Camada API
        // 1. Validação centralizada de todos os campos obrigatórios
        var errors = StudentRequestValidator.ValidateUpdateStudent(updateStudentDto);
        if (errors.Any())
            return BadRequest(new { message = "Validation failed", errors });

        try
        {
            var updatedStudent = await studentService.UpdateAsync(id, updateStudentDto);
            return Ok(updatedStudent);
        }
        catch (StudentNotFoundException)
        {
            return NotFound(new { message = $"Student with ID {id} not found" });
        }
        catch (InvalidStudentDataException ex)
        {
            logger.LogWarning(ex, "Invalid student data provided for update");
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating student with ID {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
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
        try
        {
            await studentService.DeleteAsync(id);
            return NoContent();
        }
        catch (StudentNotFoundException)
        {
            return NotFound(new { message = $"Student with ID {id} not found" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting student with ID {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
} 