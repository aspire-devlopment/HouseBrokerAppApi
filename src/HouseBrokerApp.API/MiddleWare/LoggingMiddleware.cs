using System.Diagnostics;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);

        try
        {
            await _next(context);
            stopwatch.Stop();
            _logger.LogInformation("Response: {StatusCode} processed in {ElapsedMilliseconds} ms",
                context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Exception for request {Method} {Path} after {ElapsedMilliseconds} ms",
                context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
