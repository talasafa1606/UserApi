using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserApi.Exceptions;

namespace UserApi.Filters
{
    public class GlobalExceptionHandler : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var message = "An unexpected error occurred.";

            if (context.Exception is UserApiException apiException)
            {
                statusCode = apiException.StatusCode;
                message = apiException.Message;
            }

            _logger.LogError(context.Exception,
                "An error occurred: {Message}", message);

            var errorResponse = new
            {
                Message = message,
                Status = statusCode,
                Timestamp = DateTime.UtcNow
            };

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
