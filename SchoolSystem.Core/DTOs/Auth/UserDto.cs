using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.DTOs.Auth;

public class CreateUserDto {
    public string Name { get; set; } = string.Empty;
    public SexType Sex { get; set; }
    public DateTime? Dob { get; set; }
    public string? Contact { get; set; }
    public string Password { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public List<RoleName> Roles { get; set; } = new();
}

public class UpdateUserDto {
    public string Name { get; set; } = string.Empty;
    public SexType Sex { get; set; }
    public DateTime? Dob { get; set; }
    public string? Contact { get; set; }
    public bool IsActive { get; set; } = true;
    public List<RoleName> Roles { get; set; } = new();
}

public class UserResponseDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SexType Sex { get; set; }
    public DateTime? Dob { get; set; }
    public string? Contact { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<RoleName> Roles { get; set; } = new();
}

public class LoginDto {
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto {
    public string Token { get; set; } = string.Empty;
    public UserResponseDto User { get; set; } = null!;
}