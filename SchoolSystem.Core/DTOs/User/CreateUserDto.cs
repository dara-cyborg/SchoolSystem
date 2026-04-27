using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.DTOs.User;

public class CreateUserDto {
    public string Name { get; set; } = string.Empty;
    public SexType Sex { get; set; }
    public DateTime? Dob { get; set; }
    public string? Contact { get; set; }
    public string Password { get; set; } = string.Empty;
    public List<int> RoleIds { get; set; } = new();
}
