using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.Models;

public class User : BaseEntity {
    public string Name { get; set; } = string.Empty;
    public SexType Sex { get; set; }
    public DateTime? Dob { get; set; }
    public string? Contact { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Class> HomeroomClasses { get; set; } = new List<Class>();
    public ICollection<ClassSubject> TeachingClassSubjects { get; set; } = new List<ClassSubject>();
    public ICollection<MonthlyScore> SubmittedMonthlyScores { get; set; } = new List<MonthlyScore>();
    public ICollection<MonthlyReport> SubmittedMonthlyReports { get; set; } = new List<MonthlyReport>();
    public ICollection<SemesterReport> SubmittedSemesterReports { get; set; } = new List<SemesterReport>();
    public ICollection<YearlyReport> SubmittedYearlyReports { get; set; } = new List<YearlyReport>();
    public ICollection<Feedback> ParentFeedbacks { get; set; } = new List<Feedback>();
    public ICollection<ParentStudent> ParentStudents { get; set; } = new List<ParentStudent>();
}