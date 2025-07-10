using HouseBrokerApp.WebClient.Models;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace HouseBrokerApp.WebClient.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        private const string TokenKey = "authToken";

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

             if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadFromJsonAsync<TokenResponse>();
            if (json == null || string.IsNullOrEmpty(json.Token)) return null;

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, json.Token);
            return json.Token;
        }

        public async Task<string?> RegisterAsync(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
            if (!response.IsSuccessStatusCode) return null;

            return "Registered";
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
