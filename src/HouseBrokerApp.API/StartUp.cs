using HouseBrokerApp.Application.Interfaces;
using HouseBrokerApp.Application.Services;
using HouseBrokerApp.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using HouseBrokerApp.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using Serilog;
using HouseBrokerApp.Application.Mapper;
using HouseBrokerApp.Infrastructure.Helper;

public static class StartUp
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICommissionService, CommissionService>();
        services.AddScoped<IPropertyListingService, PropertyListingService>();

        services.AddAutoMapper(typeof(PropertyListingProfile).Assembly);

        return services;
    }


    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HouseBrokerAppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<JwtTokenService>();
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<HouseBrokerAppDbContext>()
            .AddDefaultTokenProviders();


  


         var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);



        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
 ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }

    };
   
});

        services.AddAuthorization();

        return services;
    }
}
