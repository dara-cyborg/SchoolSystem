namespace SchoolSystem.Core.Models;

public class ClassSubject : BaseEntity{
    public int ClassId { get; set; }
    public int SubjectId { get; set; }
    public int? TeacherUserId { get; set; }

    // Navigation properties
    public Class Class { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
    public User? Teacher { get; set; }
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<GradebookEntry> GradebookEntries { get; set; } = new List<GradebookEntry>();
    public ICollection<MonthlyScore> MonthlyScores { get; set; } = new List<MonthlyScore>();
    public ICollection<SemesterScore> SemesterScores { get; set; } = new List<SemesterScore>();
    public ICollection<YearlyScore> YearlyScores { get; set; } = new List<YearlyScore>();
}