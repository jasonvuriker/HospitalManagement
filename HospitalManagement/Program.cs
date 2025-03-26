using HospitalManagement.Extensions;
using HospitalManagement.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var configuration = builder.Configuration;

builder.Services
    .AddDependencies()
    .AddDbContext(configuration)
    .AddConfigurations(configuration)
    .AddMonitoring(configuration);

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<CorrelationIdLoggingMiddleware>();
app.UseMiddleware<GlobalLoggingMiddleware>();
app.UseMiddleware<ConfigurationValidationMiddleware>();

app.MapControllers();

app.Run();
