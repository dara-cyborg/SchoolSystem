namespace SchoolSystem.Core.Models;

public class SemesterReportEntry : BaseEntity {
    public int ReportId { get; set; }
    public int StudentId { get; set; }
    public decimal? TotalScore { get; set; }
    public short? Rank { get; set; }

    // Navigation properties
    public SemesterReport Report { get; set; } = null!;
    public Student Student { get; set; } = null!;
}