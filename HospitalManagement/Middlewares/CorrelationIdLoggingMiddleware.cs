using Microsoft.Extensions.Options;

namespace HospitalManagement.Middlewares;

public class CorrelationIdLoggingMiddleware : IMiddleware
{
    private readonly ILogger<CorrelationIdLoggingMiddleware> _logger;

    public CorrelationIdLoggingMiddleware(ILogger<CorrelationIdLoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var scope = new Dictionary<string, string>()
        {
            { "CorrelationId", Guid.NewGuid().ToString() }
        };

        using (_logger.BeginScope(scope))
        {
            await next(context);
        }
    }
}
