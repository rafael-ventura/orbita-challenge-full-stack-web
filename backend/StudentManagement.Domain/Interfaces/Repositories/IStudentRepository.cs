using StudentManagement.Domain.Entities;

namespace StudentManagement.Domain.Interfaces.Repositories;

public interface IStudentRepository : IBaseRepository<Student>
{
    Task<Student?> GetByRAAsync(string ra);
    Task<Student?> GetByCPFAsync(string cpf);
    Task<bool> ExistsByRAAsync(string ra);
    Task<bool> ExistsByCPFAsync(string cpf);
} 