namespace SchoolSystem.Core.Models;

public class GradebookEntry : BaseEntity{
    public int ClassSubjectId { get; set; }
    public int StudentId { get; set; }
    public string Label { get; set; } = string.Empty;
    public decimal MaxScore { get; set; }
    public decimal Score { get; set; }
    public DateTime EntryDate { get; set; }

    // Navigation properties
    public ClassSubject ClassSubject { get; set; } = null!;
    public Student Student { get; set; } = null!;
}