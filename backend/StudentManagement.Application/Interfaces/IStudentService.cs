using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Interfaces;

public interface IStudentService : IBaseService<StudentDto, CreateStudentDto, UpdateStudentDto>
{
    // Métodos específicos do Student podem ser adicionados aqui
    // Por exemplo: Task<IEnumerable<StudentDto>> GetByCourseAsync(Guid courseId);
} 