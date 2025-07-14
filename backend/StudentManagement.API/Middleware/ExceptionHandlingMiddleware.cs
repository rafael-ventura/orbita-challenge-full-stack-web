using System.Net;
using System.Text.Json;
using StudentManagement.Domain.Exceptions;

namespace StudentManagement.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            StudentNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            InvalidStudentDataException => (HttpStatusCode.Conflict, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        response.StatusCode = (int)statusCode;

        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var result = JsonSerializer.Serialize(new
        {
            message = message,
            statusCode = (int)statusCode
        });

        await response.WriteAsync(result);
    }
} 