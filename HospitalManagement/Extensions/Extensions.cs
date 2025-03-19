using HospitalManagement.DataAccess;
using HospitalManagement.Middlewares;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interfaces;
using HospitalManagement.Services;
using HospitalManagement.Services.Doctors;
using HospitalManagement.Settings;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HospitalManagement.Extensions;

public static class Extensions
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddSingleton<PdpService>();
        services.AddTransient<CorrelationIdLoggingMiddleware>();

        return services;
    }

    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DoctorsSettings>()
            .BindConfiguration("DoctorsSettings")
            .ValidateDataAnnotations();

        services.AddOptions<PdpSettings>()
            .Bind(configuration.GetSection("PdpSettings"))
            .ValidateDataAnnotations();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<HospitalContext>(options =>
        {
            options
                .UseNpgsql(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .UseSnakeCaseNamingConvention();
        });

        return services;
    }

    public static IServiceCollection AddMonitoring(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((serviceProvider, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom
                .Configuration(configuration);
        });

        return services;
    }
}
