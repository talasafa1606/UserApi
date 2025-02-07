using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

public class LoggingActionFilter : IActionFilter
{
    private readonly ILogger<LoggingActionFilter> _logger;
    private Stopwatch _stopwatch;

    public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _stopwatch = Stopwatch.StartNew();
        _logger.LogInformation(
            "Action {Action} on {Controller} starting - Parameters: {@Parameters}",
            context.ActionDescriptor.DisplayName,
            context.Controller.GetType().Name,
            context.ActionArguments
        );
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch.Stop();
        _logger.LogInformation(
            "Action {Action} on {Controller} completed in {ElapsedMilliseconds}ms - Result: {@Result}",
            context.ActionDescriptor.DisplayName,
            context.Controller.GetType().Name,
            _stopwatch.ElapsedMilliseconds,
            context.Result
        );
    }
}
