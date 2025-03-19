using Microsoft.Extensions.Options;

namespace HospitalManagement.Middlewares;

public class ConfigurationValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ConfigurationValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OptionsValidationException ex)
        {
            var fails = string.Join(", ", ex.Failures);
            throw new InvalidOperationException($"Configuration validation failed {fails}");
        }
    }
}
