namespace StudentManagement.Domain.Exceptions;

public class StudentNotFoundException : BaseException
{
    public StudentNotFoundException(Guid id) : base($"Student with ID {id} was not found.")
    {
    }
}

public class StudentAlreadyExistsException : BaseException
{
    public StudentAlreadyExistsException(string ra, string cpf) : base($"Student with RA {ra} or CPF {cpf} already exists.")
    {
    }
}

public class InvalidStudentDataException : BaseException
{
    public InvalidStudentDataException(string message) : base(message)
    {
    }
} 