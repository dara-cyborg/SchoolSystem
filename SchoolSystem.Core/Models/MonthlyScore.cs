namespace SchoolSystem.Core.Models;

public class MonthlyScore : BaseEntity{
    public int ClassSubjectId { get; set; }
    public int StudentId { get; set; }
    public short Month { get; set; }
    public short SchoolYear { get; set; }
    public decimal FinalScore { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public int? SubmittedBy { get; set; }
    public bool IsLocked { get; set; }

    // Navigation properties
    public ClassSubject ClassSubject { get; set; } = null!;
    public Student Student { get; set; } = null!;
    public User? SubmittedByUser { get; set; }
}