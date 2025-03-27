using System.Reflection;
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddSingleton<PdpService>();
        services.AddTransient<CorrelationIdLoggingMiddleware>();

        //var assembly = Assembly.GetExecutingAssembly();

        //var types = assembly.GetTypes()
        //    .Where(r => r.IsClass && r.Name.EndsWith("Repository"))
        //    .ToList();

        //foreach (var type in types)
        //{
        //    var interfaceType = type.GetInterfaces()
        //        .FirstOrDefault(r => r.Name.EndsWith("Repository"));

        //    services.AddScoped(interfaceType, type);
        //}

        services.Scan(r => r.FromEntryAssembly()
            .AddClasses(filter => filter.Where(type => type.Name.EndsWith("Repository")))
            .AsMatchingInterface()
            .WithLifetime(ServiceLifetime.Scoped));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
