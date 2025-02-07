public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await LogRequest(context);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        await _next(context);

        await LogResponse(context, responseBody, originalBodyStream);
    }

    private async Task LogRequest(HttpContext context)
    {
        context.Request.EnableBuffering();

        var requestBody = string.Empty;
        using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
        {
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        var logMessage = new
        {
            Timestamp = DateTime.UtcNow,
            Method = context.Request.Method,
            Path = context.Request.Path,
            QueryString = context.Request.QueryString.ToString(),
            Headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
            Body = requestBody
        };

        _logger.LogInformation("Request: {@LogMessage}", logMessage);
    }

    private async Task LogResponse(HttpContext context, MemoryStream responseBody, Stream originalBodyStream)
    {
        responseBody.Position = 0;
        var responseContent = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Position = 0;

        var logMessage = new
        {
            Timestamp = DateTime.UtcNow,
            StatusCode = context.Response.StatusCode,
            Body = responseContent
        };

        _logger.LogInformation("Response: {@LogMessage}", logMessage);

        await responseBody.CopyToAsync(originalBodyStream);
    }
}
