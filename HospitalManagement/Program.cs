using HospitalManagement.DataAccess;
using HospitalManagement.Dtos;
using HospitalManagement.Extensions;
using HospitalManagement.Middlewares;
using HospitalManagement.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencies();

var configuration = builder.Configuration;

//var connectionStringPath = configuration["ConnectionStrings:DefaultConnection"];
//var mainConnectionString = configuration["HospitalManagement:ConnectionString:Main"];

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HospitalContext>(options =>
{
    options
        .UseNpgsql(connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .UseSnakeCaseNamingConvention();
});

builder.Services.AddHttpClient();

//var pdpSettings = configuration.GetSection("PdpSettings");
//var endpoint = pdpSettings.GetSection("Endpoint").Value;
//var children = pdpSettings.GetChildren();

//var pdpSettingsEndpoint = configuration.GetSection("PdpSettings:Endpoint");

//builder.Services.AddHttpClient<CreateDoctorDto>(options =>
//{
//    options.BaseAddress = new Uri(endpoint);
//});

builder.Services.Configure<PdpSettings>(configuration.GetSection("PdpSettings"));
builder.Services.Configure<DoctorsSettings>(configuration.GetSection("DoctorsSettings"));

//builder.Services.AddOptions<PdpSettings>("PdpSettings")
//    .Configure(options =>
//    {
//        options.
//    });


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
