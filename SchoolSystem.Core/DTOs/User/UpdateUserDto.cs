using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.DTOs.User;

public class UpdateUserDto {
    public string Name { get; set; } = string.Empty;
    public SexType Sex { get; set; }
    public DateTime? Dob { get; set; }
    public string? Contact { get; set; }
    public bool IsActive { get; set; }
}
