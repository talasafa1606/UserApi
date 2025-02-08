using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace UserApi.Filters
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, message) = exception switch
            {
                ArgumentException => (StatusCodes.Status400BadRequest, "Invalid argument provided."),
                InvalidOperationException => (StatusCodes.Status400BadRequest, "Invalid operation attempted."),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Requested resource not found."),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access."),
                NotImplementedException => (StatusCodes.Status501NotImplemented, "This functionality is not implemented."),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

            _logger.LogError(exception,
                "An error occurred: {Message}", message);

            var errorResponse = new
            {
                Message = message,
                Status = statusCode,
                Timestamp = DateTime.UtcNow
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
