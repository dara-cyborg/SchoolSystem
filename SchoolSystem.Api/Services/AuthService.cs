using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs.Auth;
using SchoolSystem.Core.Enums;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Services;

public class AuthService : IAuthService {
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration) {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto) {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Name == loginDto.Name && u.IsActive);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash)) {
            return null;
        }

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var token = GenerateJwtToken(user, roles);

        var userResponse = new UserResponseDto {
            Id = user.Id,
            Name = user.Name,
            Sex = user.Sex,
            Dob = user.Dob,
            Contact = user.Contact,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            Roles = roles
        };

        return new LoginResponseDto {
            Token = token,
            User = userResponse
        };
    }

    public string GenerateJwtToken(User user, IEnumerable<RoleName> roles) {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKeyHere12345678901234567890"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Contact ?? "")
        };

        // Add role claims
        foreach (var role in roles) {
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "SchoolSystem",
            audience: _configuration["Jwt:Audience"] ?? "SchoolSystem",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}