using System.Diagnostics;

namespace SchoolSystem.Core.Models;

public class Class : BaseEntity{
    public int GradeId { get; set; }
    public short SchoolYear { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? HomeroomUserId { get; set; }

    // Navigation properties
    public Grade Grade { get; set; } = null!;
    public User? HomeroomTeacher { get; set; }
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
    public ICollection<MonthlyReport> MonthlyReports { get; set; } = new List<MonthlyReport>();
    public ICollection<SemesterReport> SemesterReports { get; set; } = new List<SemesterReport>();
    public ICollection<YearlyReport> YearlyReports { get; set; } = new List<YearlyReport>();
}