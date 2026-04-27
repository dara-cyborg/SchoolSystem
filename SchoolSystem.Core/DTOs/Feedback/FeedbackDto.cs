using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.DTOs.Feedback;

public class FeedbackDto {
    public int Id { get; set; }
    public int ParentUserId { get; set; }
    public ReportType ReportType { get; set; }
    public int? MonthlyReportId { get; set; }
    public int? SemesterReportId { get; set; }
    public int? YearlyReportId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
