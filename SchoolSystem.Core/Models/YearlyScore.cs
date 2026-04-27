namespace SchoolSystem.Core.Models;

public class YearlyScore : BaseEntity {
    public int ClassSubjectId { get; set; }
    public int StudentId { get; set; }
    public short SchoolYear { get; set; }
    public decimal? FinalScore { get; set; }

    // Navigation properties
    public ClassSubject ClassSubject { get; set; } = null!;
    public Student Student { get; set; } = null!;
}