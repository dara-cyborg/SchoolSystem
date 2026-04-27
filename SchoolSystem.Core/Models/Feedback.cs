using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.Models;

public class Feedback : BaseEntity{
    public ReportType ReportType { get; set; }
    public int? MonthlyReportId { get; set; }
    public int? SemesterReportId { get; set; }
    public int? YearlyReportId { get; set; }
    public int ParentUserId { get; set; }
    public string Content { get; set; } = string.Empty;

    // Navigation properties
    public MonthlyReport? MonthlyReport { get; set; }
    public SemesterReport? SemesterReport { get; set; }
    public YearlyReport? YearlyReport { get; set; }
    public User Parent { get; set; } = null!;
}