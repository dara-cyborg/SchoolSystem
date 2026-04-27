namespace SchoolSystem.Core.Models;

public class Subject : BaseEntity {
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
}