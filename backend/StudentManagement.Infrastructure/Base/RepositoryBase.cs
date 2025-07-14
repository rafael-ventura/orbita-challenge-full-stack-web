using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Interfaces.Repositories;

namespace StudentManagement.Infrastructure.Base;

public class RepositoryBase<TEntity>(DbContext context) : IBaseRepository<TEntity>
    where TEntity : class
{
    protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        => await DbSet.ToListAsync();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        => await DbSet.FindAsync(id);

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        DbSet.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
        => await DbSet.FindAsync(id) != null;
}