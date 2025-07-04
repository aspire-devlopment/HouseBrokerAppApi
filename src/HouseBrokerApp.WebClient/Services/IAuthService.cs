using HouseBrokerApp.WebClient.Models;

namespace HouseBrokerApp.WebClient.Services;

public interface IAuthService
{
    Task<string?> LoginAsync(string email, string password);
    Task<string?> RegisterAsync(RegisterModel model);
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
}
