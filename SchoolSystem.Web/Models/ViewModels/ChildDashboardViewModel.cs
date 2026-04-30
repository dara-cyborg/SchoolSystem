namespace SchoolSystem.Web.Models.ViewModels;

public class ChildDashboardViewModel
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string SchoolYear { get; set; } = string.Empty;
    public string LatestReportSummary { get; set; } = string.Empty;
    public int? LatestMonthlyReportId { get; set; }
    public int? LatestSemesterReportId { get; set; }
    public int? LatestYearlyReportId { get; set; }
}
