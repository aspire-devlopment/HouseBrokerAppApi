using HouseBrokerApp.Domain.Entities;
using HouseBrokerApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HouseBrokerApp.Infrastructure.Seed;

public static class DataSeeder
{
    private static readonly string[] Roles = new[] { "Broker", "Seeker" };

    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
    public static async Task SeedCommissionRatesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HouseBrokerAppDbContext>();

        if (!dbContext.CommissionRates.Any())
        {
            var now = DateTime.UtcNow;

            var commissionRates = new List<CommissionRate>
            {
                new CommissionRate
                {
                    MinPrice = 0,
                    MaxPrice = 4999999,
                    Rate = 0.02m,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new CommissionRate
                {
                    MinPrice = 5000000,
                    MaxPrice = 10000000,
                    Rate = 0.0175m,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new CommissionRate
                {
                    MinPrice = 10000001,
                    MaxPrice = null,
                    Rate = 0.015m,
                    CreatedAt = now,
                    UpdatedAt = now
                }
            };

            dbContext.CommissionRates.AddRange(commissionRates);
            await dbContext.SaveChangesAsync();
        }
    }

    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = ["Admin", "Broker", "HouseSeeker"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create default Admin user if it doesn't exist
        var adminEmail = "admin@housebroker.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var user = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User",
                Role = "Admin",
                Password="Test@123"
            };

            var result = await userManager.CreateAsync(user, "Admin@123"); // strong password
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
