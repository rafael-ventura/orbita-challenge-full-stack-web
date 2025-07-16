using System.ComponentModel;

namespace StudentManagement.Domain.Exceptions
{
    public enum ErrorMessage
    {
        #region General
        [Description("An unexpected error occurred")] UnexpectedError,
        [Description("Validation failed")] ValidationFailed,
        #endregion

        #region Student
        [Description("Student with ID {0} not found")] StudentNotFound,
        [Description("Invalid student data")] InvalidStudentData,
        [Description("Name is required and must be at most 100 characters.")] NameRequired,
        [Description("Email is required, must be at most 100 characters and valid.")] EmailRequired,
        [Description("RA is required, must be at most 20 characters and valid.")] RaRequired,
        [Description("CPF is required, must be at most 14 characters")] CpfRequired,
        [Description("CPF inválido. Verifique se o número está correto.")] CpfInvalid,
        [Description("Student with RA {0} or CPF {1} already exists.")] StudentAlreadyExists,
        #endregion

        #region Auth
        [Description("Invalid email or password.")] InvalidCredentials,
        [Description("User already exists.")] UserAlreadyExists
        #endregion
    }

    public static class ErrorMessageExtensions
    {
        public static string GetDescription(this ErrorMessage value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = (DescriptionAttribute?)Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute));
            return attr?.Description ?? value.ToString();
        }
    }
} 