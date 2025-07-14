namespace StudentManagement.Domain.Exceptions;

public class BaseException : Exception
{
    public BaseException(string message) : base(message)
    {
    }
} 