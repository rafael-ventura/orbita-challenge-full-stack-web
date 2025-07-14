namespace StudentManagement.Application.Interfaces;

public interface IExternalCpfValidator
{
    Task<bool> IsCpfValidAsync(string cpf);
} 