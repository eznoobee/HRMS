using System.Text.Json;
using HRMS.Application.Common.Exceptions;

namespace HRMS.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, title, errors) = exception switch
        {
            ValidationException ve => (400, "Validation Error", ve.Errors),
            NotFoundException => (404, "Not Found", (IDictionary<string, string[]>?)null),
            ForbiddenException => (403, "Forbidden", (IDictionary<string, string[]>?)null),
            _ => (500, "Internal Server Error", (IDictionary<string, string[]>?)null)
        };

        context.Response.StatusCode = statusCode;

        var response = new
        {
            status = statusCode,
            title,
            detail = exception.Message,
            errors
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
