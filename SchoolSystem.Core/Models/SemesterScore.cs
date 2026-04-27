namespace SchoolSystem.Core.Models;

public class SemesterScore : BaseEntity {
    public int ClassSubjectId { get; set; }
    public int StudentId { get; set; }
    public short Semester { get; set; }
    public short SchoolYear { get; set; }
    public decimal? FinalScore { get; set; }

    // Navigation properties
    public ClassSubject ClassSubject { get; set; } = null!;
    public Student Student { get; set; } = null!;
}