using System.Threading.RateLimiting;
using HospitalManagement.Extensions;
using HospitalManagement.Middlewares;
using Microsoft.AspNetCore.RateLimiting;
using Sats.PostgreSqlDistributedCache;

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

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.PermitLimit = 5;
        options.Window = TimeSpan.FromSeconds(10);
        //options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        //options.QueueLimit = 5
    });

    //rateLimiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    //    RateLimitPartition.GetFixedWindowLimiter(
    //        partitionKey: httpContext.Connection.RemoteIpAddress.ToString(),
    //        factory: _ => new FixedWindowRateLimiterOptions
    //        {
    //            //AutoReplenishment = true,
    //            PermitLimit = 5,
    //            //QueueLimit = 0,
    //            Window = TimeSpan.FromSeconds(10)
    //        }));

    rateLimiterOptions.AddSlidingWindowLimiter("sliding", options =>
    {
        options.PermitLimit = 5;
        options.Window = TimeSpan.FromSeconds(10);
        options.SegmentsPerWindow = 2;
    });

    rateLimiterOptions.AddTokenBucketLimiter("tokenBucket", options =>
    {
        options.TokenLimit = 5;
        options.ReplenishmentPeriod = TimeSpan.FromSeconds(1);
        options.TokensPerPeriod = 2;
        options.AutoReplenishment = true;
    });
});

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", config =>
        config
            .WithOrigins("http://localhost:4201")
            .WithMethods("POST")
            .WithHeaders("x-api-key"));
});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("frontend1", config =>
//        config
//            .WithOrigins("http://localhost:4200")
//            .AllowAnyHeader()
//            .AllowAnyMethod());
//});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost";
    options.InstanceName = "local";
});

builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("frontend");

app.UseAuthorization();

app.UseMiddleware<CorrelationIdLoggingMiddleware>();
app.UseMiddleware<ConfigurationValidationMiddleware>();

app.UseRateLimiter();

app.MapControllers();

app.Run();
