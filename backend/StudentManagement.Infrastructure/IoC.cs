using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Services;
using StudentManagement.Domain.Interfaces.Repositories;
using StudentManagement.Infrastructure.Contexts;
using StudentManagement.Infrastructure.Repositories;

namespace StudentManagement.Infrastructure;

public static class IoC
{
    public static IServiceCollection AddStudentManagementInfrastructure(this IServiceCollection services)
    {
        // Registro de Repositórios
        services.AddScoped<IStudentRepository, StudentRepository>();
       
        // Registro de Serviços de Aplicação
        services.AddScoped<IStudentService, StudentService>();
       
        return services;
    }


    public static IServiceCollection AddStudentManagementInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Configuração do Banco de Dados
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Registro de Repositórios
        services.AddScoped<IStudentRepository, StudentRepository>();

        // Registro de Serviços de Aplicação
        services.AddScoped<IStudentService, StudentService>();

        return services;
    }
} 