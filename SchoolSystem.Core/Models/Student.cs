using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.Models;

public class Student : BaseEntity {
    public string Name { get; set; } = string.Empty;
    public SexType Sex { get; set; }
    public DateTime? Dob { get; set; }
    public string? Contact { get; set; }
    public int ClassId { get; set; }

    // Navigation properties
    public Class Class { get; set; } = null!;
    public ICollection<ParentStudent> ParentStudents { get; set; } = new List<ParentStudent>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<GradebookEntry> GradebookEntries { get; set; } = new List<GradebookEntry>();
    public ICollection<MonthlyScore> MonthlyScores { get; set; } = new List<MonthlyScore>();
    public ICollection<SemesterScore> SemesterScores { get; set; } = new List<SemesterScore>();
    public ICollection<YearlyScore> YearlyScores { get; set; } = new List<YearlyScore>();
    public ICollection<MonthlyReportEntry> MonthlyReportEntries { get; set; } = new List<MonthlyReportEntry>();
    public ICollection<SemesterReportEntry> SemesterReportEntries { get; set; } = new List<SemesterReportEntry>();
    public ICollection<YearlyReportEntry> YearlyReportEntries { get; set; } = new List<YearlyReportEntry>();
}