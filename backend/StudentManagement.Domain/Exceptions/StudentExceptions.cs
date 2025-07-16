using StudentManagement.Domain.Exceptions;
using static StudentManagement.Domain.Exceptions.ErrorMessageExtensions;

namespace StudentManagement.Domain.Exceptions;

#region Student
public class StudentNotFoundException : BaseException
{
    public StudentNotFoundException(Guid id) : base(string.Format(ErrorMessage.StudentNotFound.GetDescription(), id))
    {
    }
}

public class StudentAlreadyExistsException : BaseException
{
    public StudentAlreadyExistsException(string ra, string cpf) : base(string.Format(ErrorMessage.StudentAlreadyExists.GetDescription(), ra, cpf))
    {
    }
}

public class InvalidStudentDataException : BaseException
{
    public InvalidStudentDataException(string? message = null) : base(message ?? ErrorMessage.InvalidStudentData.GetDescription())
    {
    }
}
#endregion 