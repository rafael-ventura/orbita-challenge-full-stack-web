using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Infrastructure.Contexts.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
        entity.Property(e => e.RA).IsRequired().HasMaxLength(20);
        entity.Property(e => e.CPF).IsRequired().HasMaxLength(14);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.HasIndex(e => e.RA).IsUnique();
        entity.HasIndex(e => e.CPF).IsUnique();
        entity.HasIndex(e => e.Email).IsUnique();
    }
} 