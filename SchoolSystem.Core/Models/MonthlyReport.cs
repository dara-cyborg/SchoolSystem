namespace SchoolSystem.Core.Models;

public class MonthlyReport : BaseEntity{
    public int ClassId { get; set; }
    public short Month { get; set; }
    public short SchoolYear { get; set; }
    public int? SubmittedBy { get; set; }
    public DateTime? SubmittedAt { get; set; }

    // Navigation properties
    public Class Class { get; set; } = null!;
    public User? SubmittedByUser { get; set; }
    public ICollection<MonthlyReportEntry> Entries { get; set; } = new List<MonthlyReportEntry>();
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}