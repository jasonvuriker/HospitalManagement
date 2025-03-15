using HospitalManagement.DataAccess;
using HospitalManagement.Dtos;
using HospitalManagement.Extensions;
using HospitalManagement.Middlewares;
using HospitalManagement.Settings;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencies();

var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HospitalContext>(options =>
{
    options
        .UseNpgsql(connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .UseSnakeCaseNamingConvention();
});

builder.Services.AddHttpClient();

builder.Services.Configure<PdpSettings>(configuration.GetSection("PdpSettings"));
builder.Services.Configure<DoctorsSettings>(configuration.GetSection("DoctorsSettings"));

builder.Services.AddSerilog((serviceProvider, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(configuration);
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalLoggingMiddleware>();

app.MapControllers();

app.Run();
