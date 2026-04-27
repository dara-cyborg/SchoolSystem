using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.Models;

public class Role : BaseEntity {
    public RoleName Name { get; set; }

    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}