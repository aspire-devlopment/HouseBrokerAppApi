using Microsoft.AspNetCore.Identity;

namespace HouseBrokerApp.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public int BrokerId { get; set; }
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
    public string Password { get; set; }
    public string? Email { get; set; } 
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Role { get; set; } 
    public ICollection<PropertyListing>? Listings { get; set; }
}