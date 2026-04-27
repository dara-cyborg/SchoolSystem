using SchoolSystem.Core.DTOs.Auth;
using SchoolSystem.Core.Enums;
using SchoolSystem.Core.Models;

public interface IAuthService {
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    string GenerateJwtToken(User user, IEnumerable<RoleName> roles);
}