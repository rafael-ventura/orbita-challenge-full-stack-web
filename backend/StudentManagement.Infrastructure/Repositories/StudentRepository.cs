using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces.Repositories;
using StudentManagement.Infrastructure.Contexts;
using StudentManagement.Infrastructure.Base;

namespace StudentManagement.Infrastructure.Repositories;

public class StudentRepository(ApplicationDbContext context) : RepositoryBase<Student>(context), IStudentRepository
{
    public override async Task<Student?> GetByIdAsync(Guid id) => await base.GetByIdAsync(id);
    public override async Task DeleteAsync(Guid id) => await base.DeleteAsync(id);
    public override async Task<bool> ExistsAsync(Guid id) => await DbSet.AnyAsync(s => s.Id == id);

    public async Task<Student?> GetByRAAsync(string ra)
    {
        return await DbSet.FirstOrDefaultAsync(s => s.RA == ra);
    }

    public async Task<Student?> GetByCPFAsync(string cpf)
    {
        return await DbSet.FirstOrDefaultAsync(s => s.CPF == cpf);
    }

    public async Task<bool> ExistsByRAAsync(string ra)
    {
        return await DbSet.AnyAsync(s => s.RA == ra);
    }

    public async Task<bool> ExistsByCPFAsync(string cpf)
    {
        return await DbSet.AnyAsync(s => s.CPF == cpf);
    }
}