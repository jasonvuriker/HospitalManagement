namespace HospitalManagement.Middlewares;

public class GlobalLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"Executing {context.Request.Path}");

        await _next(context);

        Console.WriteLine($"Executed {context.Request.Path}");
    }
}
