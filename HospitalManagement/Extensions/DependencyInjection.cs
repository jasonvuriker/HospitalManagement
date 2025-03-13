using HospitalManagement.DataAccess;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interfaces;
using HospitalManagement.Services.Doctors;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IDoctorService, DoctorService>();

        services.AddDbContext<HospitalContext>(options =>
        {
            options
                .UseNpgsql("Server=localhost;Port=5432;Database=hospital_management;User Id=postgres;Password=postgres;")
                .UseSnakeCaseNamingConvention();
        });

        return services;
    }
}
