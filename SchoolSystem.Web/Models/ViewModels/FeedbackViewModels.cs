namespace SchoolSystem.Web.Models.ViewModels;

public class AllFeedbackViewModel
{
    public string ReportType { get; set; } = string.Empty;
    public int ReportId { get; set; }
    public string ReportLabel { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public int StudentId { get; set; }
    public string ReportLink { get; set; } = string.Empty;
    public string HistoryLink { get; set; } = string.Empty;
}

public class FeedbackHistoryViewModel
{
    public int Id { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public int ReportId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public bool IsRead { get; set; }
    public string ReportLink { get; set; } = string.Empty;
}

public class SubmitInputModel
{
    public string ReportType { get; set; } = string.Empty;
    public int ReportId { get; set; }
    public string Message { get; set; } = string.Empty;
}
