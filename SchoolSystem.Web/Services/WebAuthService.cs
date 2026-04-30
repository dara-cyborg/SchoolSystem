using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;

namespace SchoolSystem.Web.Services;

public class WebAuthService
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly string _apiBaseUrl;

    public WebAuthService(AppDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5041";
    }

    public async Task<(ClaimsPrincipal? principal, string? apiToken)> LoginAndGetTokenAsync(string username, string password)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Name == username && u.IsActive);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return (null, null);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };

        foreach (var userRole in user.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name.ToString()));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // Get JWT token from API
        var apiToken = await GetApiTokenAsync(username, password);

        return (principal, apiToken);
    }

    private async Task<string?> GetApiTokenAsync(string name, string password)
    {
        try
        {
            var payload = new { name, password };
            var httpClient = _httpClientFactory.CreateClient();
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json");
            var response = await httpClient.PostAsync(
                $"{_apiBaseUrl}/api/auth/login", content);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("token").GetString();
        }
        catch
        {
            return null;
        }
    }
}
