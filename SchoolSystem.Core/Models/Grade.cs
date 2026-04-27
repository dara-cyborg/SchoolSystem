namespace SchoolSystem.Core.Models;

public class Grade : BaseEntity{
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Class> Classes { get; set; } = new List<Class>();
}