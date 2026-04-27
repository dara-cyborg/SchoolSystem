namespace SchoolSystem.Core.DTOs.Auth;

public class CreateRoleDto {
    public string Name { get; set; } = string.Empty;
}

public class RoleResponseDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}