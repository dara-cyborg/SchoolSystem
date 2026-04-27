namespace SchoolSystem.Core.Models;

public class YearlyReportEntry : BaseEntity {
    public int ReportId { get; set; }
    public int StudentId { get; set; }
    public decimal? TotalScore { get; set; }
    public short? Rank { get; set; }

    // Navigation properties
    public YearlyReport Report { get; set; } = null!;
    public Student Student { get; set; } = null!;
}