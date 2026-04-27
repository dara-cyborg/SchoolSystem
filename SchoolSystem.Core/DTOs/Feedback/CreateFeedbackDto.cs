using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.DTOs.Feedback;

public class CreateFeedbackDto {
    public ReportType ReportType { get; set; }
    public int ReportId { get; set; }
    public string Message { get; set; } = string.Empty;
}
