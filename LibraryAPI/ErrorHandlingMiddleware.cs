using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

public class ErrorHandlingMiddleware
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string ErrorCode { get; }

        public ApiException(int statusCode, string errorCode, string message) : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }

    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        string errorCode;
        string errorMessage;

        // Customize error responses based on different types of exceptions
        if (ex is ApiException apiException)
        {
            errorCode = apiException.ErrorCode;
            errorMessage = apiException.Message;
            context.Response.StatusCode = apiException.StatusCode;
        }
        else if (ex is UnauthorizedAccessException)
        {
            errorCode = "Unauthorized";
            errorMessage = "Unauthorized access to the resource.";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        else
        {
            errorCode = "InternalServerError";
            errorMessage = "An unexpected error occurred.";
        }

        var errorResponse = new
        {
            error = new
            {
                code = errorCode,
                message = errorMessage,
                stackTrace = ex.StackTrace
            }
        };

        // Serialize the response object to JSON and write it to the response
        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
