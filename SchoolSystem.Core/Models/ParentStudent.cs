namespace SchoolSystem.Core.Models;

public class ParentStudent : BaseEntity {
    public int ParentUserId { get; set; }
    public int StudentId { get; set; }

    public User Parent { get; set; } = null!;
    public Student Student { get; set; } = null!;
}