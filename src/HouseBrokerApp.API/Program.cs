using FluentValidation.AspNetCore;
using HouseBrokerApp.Infrastructure;
using HouseBrokerApp.Infrastructure.Persistence;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplicationServices();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructure(builder.Configuration);



builder.Services.AddInfrastructureServices(builder.Configuration);  // from Infrastructure project
builder.Services.AddMemoryCache();


builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>();
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", builder =>
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()                        // adjust as needed
    .WriteTo.Console()                           // still write to console
    .WriteTo.File(                               // write to rolling daily file
        path: "Logs/error-.log",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error
    )
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "House Broker API",
        Version = "v1"
    });

    // Add JWT Authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token like: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await HouseBrokerApp.Infrastructure.Seed.DataSeeder.SeedRolesAndAdminAsync(services);
}
//seed comission
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await HouseBrokerApp.Infrastructure.Seed.DataSeeder.SeedCommissionRatesAsync(services);

}
//seed Roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await HouseBrokerApp.Infrastructure.Seed.DataSeeder.SeedRolesAsync(services);
}




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("NewPolicy"); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
