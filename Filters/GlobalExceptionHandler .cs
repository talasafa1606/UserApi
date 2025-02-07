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
            var result = new ObjectResult(new
            {
                Message = context.Exception.Message,
                Type = context.Exception.GetType().Name,
                context.Exception.StackTrace
            });

            switch (context.Exception)
            {
                case UserApiException userApiEx:
                    result.StatusCode = userApiEx.StatusCode;
                    break;
                case KeyNotFoundException:
                    result.StatusCode = StatusCodes.Status404NotFound;
                    break;
                case ArgumentException:
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                case UnauthorizedAccessException:
                    result.StatusCode = StatusCodes.Status401Unauthorized;
                    break;
                default:
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            _logger.LogError(context.Exception, "An error occurred while processing the request");
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
